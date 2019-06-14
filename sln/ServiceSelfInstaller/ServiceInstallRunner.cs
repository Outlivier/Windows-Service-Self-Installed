using System.Collections;
using System.Configuration.Install;
using System.Reflection;

namespace ServiceSelfInstaller
{
	public class ServiceInstallRunner
	{
		/// <summary>Nom de la clé avec laquelle le nom du service </summary>
		public const string ServiceNameAppSettingsKey = "ServiceName";


		/// <summary>
		/// Lance l'installation du service.
		/// </summary>
		/// <remarks><c>virtual void</c> afin de faciliter les tests unitaires.</remarks>
		/// <param name="serviceAssembly">Assembly du service. Faire <c>Assembly.GetExecutingAssembly()</c> pour l'obtenir.</param>
		/// <param name="args">Paramètres d'installation.</param>
		public virtual void Install(Assembly serviceAssembly, InstallArgs args)
		{
			using (var installer = new ServiceInstallerComponent(serviceAssembly, args))
			using (var ti = new TransactedInstaller())
			{
				ti.Installers.Add(installer);
				ti.Context = new InstallContext(logFilePath: null, commandLine: new string[] { "/assemblypath=" + installer.ServiceAssemblyPath });
				ti.Install(new Hashtable());
			}
		}

		/// <summary>
		/// Lance la désinstallation du service.
		/// </summary>
		/// <remarks><c>virtual void</c> afin de faciliter les tests unitaires.</remarks>
		/// <param name="serviceAssembly">Assembly du service. Faire <c>Assembly.GetExecutingAssembly()</c> pour l'obtenir.</param>
		public virtual void Uninstall(Assembly serviceAssembly)
		{
			using (var installer = new ServiceInstallerComponent(serviceAssembly))
			using (var ti = new TransactedInstaller())
			{
				ti.Installers.Add(installer);
				ti.Context = new InstallContext(logFilePath: null, commandLine: new string[] { "/assemblypath=" + installer.ServiceAssemblyPath });
				ti.Uninstall(savedState: null);
			}
		}
	}
}
