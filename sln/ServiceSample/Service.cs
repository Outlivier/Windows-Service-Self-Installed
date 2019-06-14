using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace ServiceSample
{
	public partial class Service : ServiceBase
	{
		public Service()
		{
			InitializeComponent();
			this.ServiceName = Program.ServiceName;
			this.CanPauseAndContinue = true;
		}


		protected override void OnStart(string[] args)
		{
			LogEvent(Invariant($"Service {this.ServiceName} OnStart {DateTime.Now}"));
		}

		protected override void OnStop()
		{
			// Simule un service qui met du temps à s'arrêter
			Thread.Sleep(TimeSpan.FromSeconds(10));
			LogEvent(Invariant($"Service {this.ServiceName} OnStop {DateTime.Now}"));
		}

		protected override void OnPause()
		{
			LogEvent(Invariant($"Service {this.ServiceName} OnPause {DateTime.Now}"));
		}

		protected override void OnContinue()
		{
			LogEvent(Invariant($"Service {this.ServiceName} OnContinue {DateTime.Now}"));
		}


		private static void LogEvent(string message)
		{
			if (string.IsNullOrWhiteSpace(message)) { throw new ArgumentNullException(message); }
			using (EventLog eventLog = new EventLog("Application"))
			{
				eventLog.Source = Program.ServiceName;
				eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
			}
		}
	}
}
