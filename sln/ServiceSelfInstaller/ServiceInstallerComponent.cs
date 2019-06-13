using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.ServiceProcess;
using static System.FormattableString;

namespace ServiceSelfInstaller
{
	[RunInstaller(true)]
	internal partial class ServiceInstallerComponent : Installer
	{
		#region Constructeur - Champs privés
		/// <summary>
		/// Constructeur pour une désinstallation.
		/// </summary>
		/// <param name="serviceAssembly">Assembly du service à installer. Faire <c>Assembly.GetExecutingAssembly()</c>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceAssembly"/> est nul.</exception>
		public ServiceInstallerComponent(Assembly serviceAssembly)
		{
			// Cet appel est requis par le Concepteur de composants.
			InitializeComponent();

			// Bind des évènements
			BeforeUninstall += new InstallEventHandler(OnBeforeUninstall);
			AfterInstall += new InstallEventHandler(OnAfterInstall);

			// Récupération des paramètres
			this.ServiceAssemblyPath = (serviceAssembly ?? throw new ArgumentNullException(nameof(serviceAssembly))).Location;
		}

		/// <summary>
		/// Constructeur pour une Installation.
		/// </summary>
		/// <param name="serviceAssembly">Assembly du service à installer. Faire <c>Assembly.GetExecutingAssembly()</c>.</param>
		/// <param name="args">Paramètres de l'installation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceAssembly"/> est nul.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="args"/> est nul.</exception>
		public ServiceInstallerComponent(Assembly serviceAssembly, InstallArgs args) : this(serviceAssembly)
		{
			this.Args = args ?? throw new ArgumentNullException(nameof(args));
		}
		#endregion


		private static void Log(string message) => Console.WriteLine(message);


		private static void LogDebug(string message)
		{
#if DEBUG
			Console.WriteLine("DEBUG - " + message);
#endif
		}


		public string ServiceAssemblyPath { get; }
		public InstallArgs Args { get; }

		/// <summary>
		/// Lit ou écrit le nom du service dans le App.Config du service.
		/// </summary>
		private string ServiceName
		{
			get
			{
				var appConfig = ConfigurationManager.OpenExeConfiguration(this.ServiceAssemblyPath);
				return appConfig.AppSettings.Settings[ServiceInstallRunner.ServiceNameAppSettingsKey]?.Value;
			}
			set
			{
				var appConfig = ConfigurationManager.OpenExeConfiguration(this.ServiceAssemblyPath);
				appConfig.AppSettings.Settings.Remove(ServiceInstallRunner.ServiceNameAppSettingsKey);
				appConfig.AppSettings.Settings.Add(ServiceInstallRunner.ServiceNameAppSettingsKey, value);
				appConfig.Save(ConfigurationSaveMode.Modified);
			}
		}


		#region Install
		public override void Install(IDictionary stateSaver)
		{
			//) Configure les installers	
			Installers.Add(CreateProcessInstaller());
			Installers.Add(CreateServiceInstaller());

			//) Lance l'installation
			base.Install(stateSaver);

			//) Sauvegarde le nom du service dans le app.config pour la désinstallation.
			this.ServiceName = Args.Name;

			//) Exemple s'il l'on veut créer des EventSource supplémentaires lors de l'installation du service
			//if (string.IsNullOrWhiteSpace(Configuration.EventLogSource)) return;
			//if (EventLog.SourceExists(Configuration.EventLogSource)) return;
			//EventLog.CreateEventSource(Configuration.EventLogSource, "Application");
		}


		/// <summary>Restaure l'état de l'ordinateur préalable à l'installation.</summary>
		/// <param name="savedState"><see cref="IDictionary"/> qui contient l'état qui était celui de l'ordinateur
		/// avant l'installation.</param>
		/// <exception cref="ArgumentException">
		/// Le paramètre <paramref name="savedState"/> a la valeur null. ou L’état enregistré
		/// <see cref="IDictionary"/> peut être endommagé.
		/// </exception>
		/// <exception cref="System.Configuration.Install.InstallException">
		/// Une exception s’est produite lors de la <see cref="Configuration.Install.Installer.Rollback(IDictionary)"/> phase
		/// de l’installation. Cette exception est ignorée et la restauration continue.
		/// Toutefois, l’ordinateur peut ne revienne pas totalement à son état initial une fois la restauration terminée.
		/// </exception>
		public override void Rollback(IDictionary savedState)
		{
			// N'est appelé qu'en cas de problème lors de l'installation. N'est pas appelé lors de la désinstallation.
			base.Rollback(savedState);
		}


		private static void StartSC(string args)
		{
			int exitCode;
			Log("sc " + args);
			using (var process = new Process())
			{
				var startInfo = process.StartInfo;
				startInfo.FileName = "sc";
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				startInfo.Arguments = args;

				process.Start();
				process.WaitForExit();

				exitCode = process.ExitCode;
			}

			if (exitCode != 0) { throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Locale.ServiceInstallerComponent_StartSC_Error, exitCode)); }
		}


		/// <summary>
		/// Si demandé par la ligne de commande :
		/// <list>
		/// <item>Modifie les options devant êtres changées via sc.exe.</item>
		/// <item>Démarre le service car même si <see cref="ServiceInstaller.StartType"/> est <see cref="ServiceStartMode.Automatic"/>,
		/// le service n'est pas démarré par défaut après l'installation.</item>
		/// </list>
		/// </summary>
		/// <param name="sender">Source de l'événement.</param>
		/// <param name="e"><see cref="InstallEventArgs"/> qui contient les données d'événement.</param>
		private void OnAfterInstall(object sender, InstallEventArgs e)
		{
			//) Recovery Actions
			if (!string.IsNullOrWhiteSpace(Args.SCFailure))
			{
				// A noter que l'on échappe pas les " dans le nom du service (qui nomerait un service avec des guillemets ?
				StartSC(Invariant($"failure {Args.Name} {Args.SCFailure}")); 
			}

			//) Démarre le service
			if (Args.Start)
			{
				Log(Locale.ServiceSelfInstaller_OnAfterInstall_Start);
				using (var sc = new ServiceController(Args.Name))
				{
					sc.Start();
					// Inutile d'attendre le démarrage du service
				}
			}
		}


		private ServiceProcessInstaller CreateProcessInstaller()
		{
			if (Args == null)
			{
				throw new NullReferenceException(string.Format(CultureInfo.InvariantCulture, Locale.InstallArgs_CreateProcessInstaller_ArgsNullEx, nameof(Args), nameof(ServiceInstallerComponent)));
			}

			return new ServiceProcessInstaller()
			{
				Account = Args.Account,
				Username = Args.UserName,
				Password = Args.Password
			};
		}


		private ServiceInstaller CreateServiceInstaller()
		{
			ServiceInstaller serviceInstaller = new ServiceInstaller()
			{
				ServiceName = Args.Name
			};
			// DisplayName
			if (!string.IsNullOrWhiteSpace(Args.DisplayName))
			{
				serviceInstaller.DisplayName = Args.DisplayName;
			}
			// Description
			if (!string.IsNullOrWhiteSpace(Args.Description))
			{
				serviceInstaller.Description = Args.Description;
			}
			// DependsOn
			/* if ((this.CommandLine.DependsOn.Count > 0)) sInstaller.ServicesDependedOn = this.CommandLine.DependsOn.ToArray; */
			// StartType
			serviceInstaller.StartType = Args.StartMode;

			return serviceInstaller;
		}
		#endregion


		#region Uninstall
		/// <summary>
		/// Sous Windows 10, la désinstallation ne fonctionne pas si le service est démarré.
		/// </summary>
		private void OnBeforeUninstall(object sender, InstallEventArgs e)
		{
			Log(Locale.ServiceSelfInstaller_OnBeforeUninstall_Stop);
			using (var sc = new ServiceController(this.ServiceName))
			{
				if ((sc.Status != ServiceControllerStatus.Stopped) && (sc.Status != ServiceControllerStatus.StopPending))
				{
					sc.Stop();
				}
				sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
			}
		}

		public override void Uninstall(IDictionary savedState)
		{
			Installers.Add(new ServiceProcessInstaller());

			//) Il faut créer "un ServiceInstaller"
			var serviceInstaller = new ServiceInstaller()
			{
				ServiceName = this.ServiceName
			};
			Installers.Add(serviceInstaller);

			//) Lance la désinstallation
			base.Uninstall(savedState);

			//) Exemple si l'on créait un LogEventSource, qu'il faudrait supprimer lors de la désinstall
			//if (string.IsNullOrWhiteSpace(Configuration.EventLogSource)) return;
			//if (!EventLog.SourceExists(Configuration.EventLogSource)) return;
			//EventLog.DeleteEventSource(Configuration.EventLogSource);
		}
		#endregion
	}
}