namespace ServiceSelfInstaller
{
	/// <summary>
	/// Options de récupération du service.
	/// </summary>
	public class Recovery
	{

		/// <summary>Première défaillance.</summary>
		public RecoveryAction FirstFailure { get; set; } = RecoveryAction.None;
		/// <summary>Deuxième défaillance.</summary>
		public RecoveryAction SecondFailure { get; set; } = RecoveryAction.None;
		/// <summary>Défaillances suivantes.</summary>
		public RecoveryAction SubsequentFailure { get; set; } = RecoveryAction.None;

		/// <summary>
		/// Réinitialiser le compteur de défaillance après cette valeur en jours.
		/// </summary>
		/// <remarks>
		/// Setting "Reset fail count after:" to 0 means "reset the fail count to 0 after each failure".
		/// This effectively disables both the "second failure" and "subsequent failure" actions and you will always
		/// get the "first failure" action.
		/// </remarks>
		public int ResetFailCountAfter { get; set; } = 1;

		/// <summary>
		/// Redémarrer le service après cette valeur en minutes.
		/// </summary>
		public int RestartServiceAfter { get; set; } = 0;
	}
}
