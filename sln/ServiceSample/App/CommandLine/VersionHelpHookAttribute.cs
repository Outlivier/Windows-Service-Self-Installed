using PowerArgs;
using System;
using System.Reflection;
using static System.FormattableString;

namespace ServiceSample.App.CommandLine
{
	/// <summary>
	/// Permet d'afficher la version de l'application.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public sealed class VersionHelpHookAttribute : HelpHook
	{
		public override void AfterCancel(HookContext context)
		{
			// On n'affiche la version que si version est passé
			if (true.Equals(context?.CurrentArgument.RevivedValue))
			{
				var bootInfos = new AssemblyInformations(Assembly.GetExecutingAssembly());
				string version = Invariant($"{bootInfos.Title} {bootInfos.Version}");
				PowerArgs.ConsoleProvider.Current.WriteLine(new ConsoleString(version, ConsoleColor.Green));
			}
		}
	}
}