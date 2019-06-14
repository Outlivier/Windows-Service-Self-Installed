using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using ServiceSelfInstaller;

namespace ServiceTests
{
	[TestFixture]
	[Category("Service")]
	public class InstallArgsT
	{
		[Test]
		public void Constructor_ShouldThrow_WhenNameIsNullOrWhitespace()
		{
			Action an = () => new InstallArgs(name: null);
			Action ae = () => new InstallArgs(name: "");
			Action aw = () => new InstallArgs(name: "   ");
			an.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("name");
			ae.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("name");
			aw.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("name");
		}


		[Test]
		public void Constructor_ShouldThrow_WhenNameIsTooLong()
		{
			//) Arrange
			var rnd = new Random();
			var name = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 250).Select(s => s[rnd.Next(s.Length)]).ToArray());
			//) Act
			Action construct = () => new InstallArgs(name: name);
			//) Assert
			construct.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("name");
		}
	}
}
