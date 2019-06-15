using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using ServiceSelfInstaller;
using PowerArgs;
using FakeItEasy;

namespace ServiceTests
{
	[TestFixture]
	[Category("Service")]
	public class ProgramT
	{
		#region Setup - Teardown
		/// <summary>
		/// Initialisation avant l'exécution de tous les tests de la classe.
		/// </summary>
		[OneTimeSetUp]
		public void SetupBeforeAll()
		{
			IConsoleProvider fakeConsole = A.Fake<IConsoleProvider>();
			A.CallTo(() => fakeConsole.WriteLine(A<ConsoleString>._)).Invokes((ConsoleString s) => { ConsoleOutput.Add(s.StringValue); });
			ConsoleProvider.Current = fakeConsole;
		}


		/// <summary>
		/// Initialisation avant l'exécution de chaque test.
		/// </summary>
		[SetUp]
		public void SetupBeforeEach()
		{
			ConsoleOutput = new List<string>();
		}


		[ThreadStatic]
		static List<string> ConsoleOutput;
		#endregion


		#region Tests
		[Test]
		public void Main_ShouldPrintVersion_WhenVersionShitch()
		{
			//) Act
			var exitCode = ServiceSample.Program.Main(new string[] { "-version" });
			//) Assert
			ConsoleOutput.Should().HaveCount(1);
			ConsoleOutput[0].Should().MatchRegex(@"^ServiceSample (\d+\.){1,3}\d+$");
		}
		#endregion
	}
}
