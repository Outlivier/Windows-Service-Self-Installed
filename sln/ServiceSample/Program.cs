using PowerArgs;
using ServiceSample.App.CommandLine;
using ServiceSelfInstaller;
using System;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;

namespace ServiceSample
{
	static class Program
	{
		public static string ServiceName => ConfigurationManager.AppSettings[ServiceInstallRunner.ServiceNameAppSettingsKey] ?? "1_ServiceSample";
		public static ServiceInstallRunner Installer { get; set; } = new ServiceInstallRunner();


		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		static int Main(string[] args)
		{
			var exitCode = ExitCode.Error;
			var runMode = RunMode.Canceled;
			ArgAction<App.CommandLine.Args> action = null;

			//) Parse la ligne de commande
			// Avec PowerArgs, on ne peut pas avoir une action par défaut => si args est empty, action = exe
			if (args.Length == 0)
			{
				runMode = RunMode.Exe;
			}
			else
			{
				action = PowerArgs.Args.InvokeAction<App.CommandLine.Args>(args);
				if (action.HandledException == null)
				{
					if (action.Cancelled)
					{
						// cancelled est vrai quand l'aide ou la version est affichée
						exitCode = ExitCode.Canceled;
					}
					else
					{
						// Action (Install / Uninstall)					
						runMode = action.Args.ParsedRunMode;
					}
				}
			}

			//) Installation / Désinstallation / Demarrage normal
			switch (runMode)
			{
				case RunMode.Install:
					Installer.Install(Assembly.GetExecutingAssembly(), action.Args.ParsedInstallArgs);
					exitCode = ExitCode.Success;
					break;
				case RunMode.Uninstall:
					Installer.Uninstall(Assembly.GetExecutingAssembly());
					exitCode = ExitCode.Success;
					break;
				case RunMode.Exe:
					{
						ServiceBase[] ServicesToRun;
						ServicesToRun = new ServiceBase[] { new Service() };
						ServiceBase.Run(ServicesToRun);
					}
					break;
			}

			//) Fin
			return (int)exitCode;
		}
	}
}