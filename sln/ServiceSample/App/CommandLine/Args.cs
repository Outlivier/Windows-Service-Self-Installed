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
	[ArgExample("MyApplication.Cli.exe", "Exécute le service :")]
	[ArgExample("MyApplication.Cli.exe -Version", "Affiche le n° de version :")]
	[ArgExample("MyApplication.Cli.exe Uninstall", "Désinstalle le service :")]
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
		[ArgDescription("Affiche l'aide de la ligne de commande.\n")]
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
