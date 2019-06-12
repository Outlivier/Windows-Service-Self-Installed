using PowerArgs;
using System.ServiceProcess;

namespace ServiceSample.App.CommandLine
{
	[ArgIgnoreCase(ignore: true)]
	public class ArgsInstall
	{
		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("n")]
		[ArgDescription("Nom du service.")]
		public string Name { get; set; }

		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("dn")]
		[ArgDescription("Indique le nom d'affiche qui permet à l'utilisateur d'identifier le service.")]
		public string DisplayName { get; set; }

		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("d")]
		public string Description { get; set; }

		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("s")]
		[ArgDefaultValue(ServiceStartMode.Automatic)]
		public ServiceStartMode StartMode { get; set; }

		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("a")]
		[ArgDefaultValue(ServiceAccount.NetworkService)]
		[ArgDescription("Compte avec lequel le service sera exécuté. Pour un compte de type user, les paramètres UserName et Password sont obligatoires.")]
		public ServiceAccount Account { get; set; }

		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("u")]
		public string UserName { get; set; }

		/// <remarks>
		/// On n'utilise pas <see cref="SecureStringArgument"/>, car cet attribut force le prompt du mot de passe.
		/// Si l'on veut un prompt du mot de passe, ne pas spécifier ce paramètre et laisser le service afficher une fenêtre de
		/// saisie des identifiants lors de l'installation.
		/// </remarks>
		[ArgNotEmptyOrWhiteSpace()]
		[ArgShortcut("p")]
		public string Password { get; set; }


		public ServiceSelfInstaller.InstallArgs ToServiceArgs()
		{
			return new ServiceSelfInstaller.InstallArgs(this.Name ?? Program.ServiceName)
			{
				Account = this.Account,
				Description = this.Description,
				DisplayName = this.DisplayName,
				Password = this.Password,
				StartMode = this.StartMode,
				UserName = this.UserName
			};
		}
	}
}