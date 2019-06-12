using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSample.App.CommandLine
{
	public enum ExitCode : int
	{
		Error = -1,
		Success = 0,
		Canceled = 1
	}
}
