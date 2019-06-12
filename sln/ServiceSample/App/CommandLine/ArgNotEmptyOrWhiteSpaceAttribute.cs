using PowerArgs;
using System;
using System.Globalization;

namespace ServiceSample.App.CommandLine
{
	/// <summary>
	/// <para>
	/// Indique que ce paramètre de type string de la ligne de commande ne peut être vide ou composé uniquement d'espaces blancs.
	/// </para>
	/// <para>
	/// A noter que l'on ne vérifie pas si l'argument est nul, utiliser <see cref="ArgRequired"/>.
	/// A partir du moment où l'argument est spécifié dans la ligne de commande, PowerArgs passe une chaîne vide
	/// si aucune valeur n'est passée.
	/// </para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public sealed class ArgNotEmptyOrWhiteSpaceAttribute : ArgValidator
	{
		public override void Validate(string name, ref string arg)
		{
			if (string.IsNullOrWhiteSpace(arg))
			{
				var msg = string.Format(CultureInfo.InvariantCulture, Locale.App_CommandLine_ArgNotEmptyOrWhiteSpaceAttribute, name);
				throw new ValidationArgException(msg);
			}
		}
	}
}