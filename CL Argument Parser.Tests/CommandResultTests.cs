using NUnit.Framework;

namespace CLAP.Tests.CommandTestsNS
{
	[TestFixture]
	public class CommandTests
	{
		[Test]
		public void SimpleSW_AcceptsArg()
		{
			var setup = new Setup();
			var parser = new Parser(setup);
			var parserResult = parser.Parse(new string[] {"p1", "-s", "switch-arg", "--switch",  });
			var sw1 = new CommandSwitch("switch", acceptsArguments: true);
			sw1.AddAlternativeName("s");
			var command = new Command(setup);
			command.AddSwitch(sw1);
			var result = command.Adapt(parserResult);

			Assert.AreEqual(1, result.paths.Count);
			Assert.AreEqual("p1", result.paths[0]);
			Assert.AreEqual(1, result.switches.Count);
			Assert.AreEqual("switch-arg", result.switches[sw1][0]);
		}

		[Test]
		public void SimpleSW_NoArg()
		{
			var setup = new Setup();
			var parser = new Parser(setup);
			var parserResult = parser.Parse(new string[] {"-s", "--switch", "commandArg" });
			var sw1 = new CommandSwitch("switch", acceptsArguments: false);
			sw1.AddAlternativeName("s");
			var command = new Command(setup);
			command.AddSwitch(sw1);
			var result = command.Adapt(parserResult);

			Assert.AreEqual(1, result.paths.Count);
			Assert.AreEqual("commandArg", result.paths[0]);
			Assert.AreEqual(0, result.switches.Count);
		}
	}
}
