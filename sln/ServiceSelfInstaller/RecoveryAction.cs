namespace ServiceSelfInstaller
{
	/// <summary>
	/// Type de réponse possible de l'ordinateur en cas de défaillance du service.
	/// </summary>
	public enum RecoveryAction
	{
		/// <summary>Ne rien faire.</summary>
		None = 0,
		/// <summary>Redémarrer le service.</summary>
		RestartService,
		/// <summary>Exécuter un programme.</summary>
		RunCommand,
		/// <summary>Redémarrer l'ordinateur.</summary>
		Reboot
	}
}
