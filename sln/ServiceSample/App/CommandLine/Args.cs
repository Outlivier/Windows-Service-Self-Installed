using PowerArgs;

namespace ServiceSample.App.CommandLine
{
	/// <summary>
	/// Arguments de la ligne de commande.
	/// </summary>
	/// <remarks>
	/// <list>
	/// <item>On n'utilise pas <see cref="TabCompletion"/> car la saisie automatique PowerArgs fonctionne mal dans une console powerShell.</item>
	/// <item>Les chaînes dans les attributs comme <see cref="ArgDescription"/> sont saisies directement en Français, car on ne peut
	/// pas utiliser un fichier de Ressource dans un attribut. En cas de necessité d'internationalisation, soit dupliquer les classes et
	/// charger les instances en fonction de <see cref="System.Threading.Thread.CurrentUICulture"/>, ou créer les arguments "à la main"
	/// via <see cref="CommandLineArgumentsDefinition"/>.</item>
	/// </list>
	/// </remarks>
	[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
	[ArgIgnoreCase(ignore: true)]
	[ArgExample("ServiceSample.exe", "Exécute le service :")]
	[ArgExample("ServiceSample.exe -Version", "Affiche le n° de version :")]
	[ArgExample("ServiceSample.exe Install -n \"OtherServiceName\"", "Installe le service avec un nom différent :")]
	[ArgExample(@"ServiceSample.exe i -scf ""reset= 172800 actions= restart/180000/run/300000/restart/0 command= \""powershell -command gci | Out-File \\\""C:\_\Un fichier.txt\\\"";\""""",
		"Définit les options de récupérations en lançant la commande dans une fenêtre DOS. On montre ici comment échapper les guillements dans les arguments de la commande sc.exe, " +
		"et dans le paramètre scf du service :")]
	[ArgExample(@"&"".\ServiceSample.exe"" install -scf 'reset= 172800 actions= restart/180000/run/300000/restart/0 command= \""powershell -command gci | Out-File \\\""C:\_\Un fichier.txt\\\"";\""'",
		"Définit les options de récupérations en lançant la commande dans une fenêtre PowerShell. On montre ici comment échapper les guillements dans les arguments de la commande sc.exe, " +
		"et dans le paramètre scf du service, et en utilisant les simples Quotes PowerShell :")]
	[ArgExample("ServiceSample.exe Uninstall", "Désinstalle le service :")]
	public class Args
	{
		/// <remarks>A mettre en premier avant les autres arguments.</remarks>
		[VersionHelpHook()]
		[ArgDescription("Affiche la version de l'application")]
		[ArgShortcut(ArgShortcutPolicy.NoShortcut)]
		public bool Version { get; set; }

		/// <remarks>A mettre juste après <see cref="Version"/>.</remarks>
		[HelpHook()]
		[ArgShortcut("h")]
		[ArgShortcut("?")]
		[ArgDescription("Affiche l'aide de la ligne de commande.")]
		public bool Help { get; set; }

		[ArgActionMethod]
		[ArgShortcut("i")]
		[ArgDescription("Installe le service.")]
		public void Install(ArgsInstall args)
		{
			this.ParsedRunMode = RunMode.Install;
			this.ParsedInstallArgs = args.ToServiceArgs();
		}

		[ArgActionMethod]
		[ArgShortcut("u")]
		[ArgDescription("Désinstalle le service.")]
		public void Uninstall() => this.ParsedRunMode = RunMode.Uninstall;

		[ArgIgnore()]
		public RunMode ParsedRunMode { get; private set; } = RunMode.Exe;

		[ArgIgnore()]
		public ServiceSelfInstaller.InstallArgs ParsedInstallArgs { get; set; }


		/// <summary>Point d'entrée, on ne s'en sert pas ici, mais évite que PowerArgs lève une exception.</summary>
		public void Main() { }
	}
}
