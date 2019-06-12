using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
		}


		protected override void OnStart(string[] args)
		{
			LogEvent(Invariant($"Service {this.ServiceName} OnStart {DateTime.Now}"));
		}

		protected override void OnStop()
		{
			LogEvent(Invariant($"Service {this.ServiceName} OnStop {DateTime.Now}"));
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
