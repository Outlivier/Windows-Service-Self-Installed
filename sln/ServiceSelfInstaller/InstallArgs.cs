using System;
using System.Globalization;
using System.ServiceProcess;

namespace ServiceSelfInstaller
{
	/// <summary>
	/// Arguments à fournir lors de l'installation du service.
	/// </summary>
	public class InstallArgs
	{
		/// <summary>
		/// Constructeur minimal. Seul le nom du service est obligatoire lors de l'installation.
		/// </summary>
		/// <param name="name">Nom du service. Ce nom peut être différent de <see cref="ServiceBase.ServiceName"/>.</param>
		public InstallArgs(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }
			if (name.Length > ServiceBase.MaxNameLength)
			{
				throw new ArgumentOutOfRangeException(
					nameof(name),
					name,
					string.Format(CultureInfo.InvariantCulture, Locale.InstallArgs_Constructor_NameCheck, ServiceBase.MaxNameLength));
			}
			this.Name = name;
		}


		public string Name { get; }
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public ServiceStartMode StartMode { get; set; } = ServiceStartMode.Automatic;

		/// <summary>
		/// <para>Compte avec lequel le service sera exécuté.</para>
		/// <para>
		/// Si la valeur est <see cref="ServiceAccount.User"/>, on peut fournir les identifiants via les propriétés
		/// <see cref="UserName"/> et <see cref="Password"/>. Si le nom d'utiliser et/ou le mot de passe n'est pas fourni,
		/// Windows affichera une boîte de dialogue permettant de les saisir lors de l'installation du service.
		/// </para>
		public ServiceAccount Account { get; set; } = ServiceAccount.NetworkService;

		public string UserName { get; set; }

		/// <remarks>
		/// On n'utilise pas une <see cref="System.Security.SecureString"/> car <see cref="ServiceProcessInstaller.Password"/>
		/// est une chaîne en clair.
		/// </remarks>
		public string Password { get; set; }
	}
}
