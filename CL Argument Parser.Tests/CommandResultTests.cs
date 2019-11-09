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
			var sw1 = new CommandSwitch("switch");
			sw1.AddArgument("sw-arg", isOptional: false);
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
			var sw1 = new CommandSwitch("switch");
			sw1.AddAlternativeName("s");
			var command = new Command(setup);
			command.AddSwitch(sw1);
			var result = command.Adapt(parserResult);

			Assert.AreEqual(1, result.paths.Count);
			Assert.AreEqual("commandArg", result.paths[0]);
			Assert.AreEqual(0, result.switches.Count);
		}
		
		[Test]
		public void Help_Test()
		{
			var setup = new Setup();

			var sw1 = new CommandSwitch("no-post-rewrite-for-whatever-reason-very-long");
			sw1.AddAlternativeName("n");
			sw1.AddHelp("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.");

			var sw2 = new CommandSwitch("s");
			sw2.AddHelp("At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga.");

			var command = new Command(setup);
			command.AddSwitch(sw1);
			command.AddSwitch(sw2);

			var actual = command.GetHelp();
			var expected = @"All available switches:

    --no-post-rewrite-for-whatever-reason-very-long, -n
                          Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
    -s                    At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga.
".Replace("\r\n", "\n");
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void OptionalArgument()
		{
			var setup = new Setup();
			var parser = new Parser(setup);
			var parserResult = parser.Parse(new string[] {"p1", "-s", "switch-arg", "--switch",  });
			var sw1 = new CommandSwitch("switch");
			sw1.AddArgument("sw-arg", isOptional: true);
			sw1.AddAlternativeName("s");
			var command = new Command(setup);
			command.AddSwitch(sw1);
			var result = command.Adapt(parserResult);

			Assert.AreEqual("switch-arg", result.switches[sw1][0]);
		}

		[Test]
		public void TooManyArguments_Throws()
		{
			var setup = new Setup();
			var parser = new Parser(setup);
			var parserResult = parser.Parse(new string[] {"p1", "-s", "switch-arg", "--switch", "another" });
			var sw1 = new CommandSwitch("switch");
			sw1.AddArgument("sw-arg", isOptional: true);
			sw1.AddAlternativeName("s");
			var command = new Command(setup);
			command.AddSwitch(sw1);

			Assert.Throws<InputException>(() => command.Adapt(parserResult));
		}

		[Test]
		public void Argument_Help()
		{
			var setup = new Setup();

			var sw1 = new CommandSwitch("n");
			sw1.AddArgument("ag1");
			sw1.AddArgument("ag2", isOptional: true);
			sw1.AddHelp("Lorem");

			var command = new Command(setup);
			command.AddSwitch(sw1);

			var actual = command.GetHelp();
			var expected = @"All available switches:

    -n <ag1> [ag2]        Lorem
".Replace("\r\n", "\n");
			Assert.AreEqual(expected, actual);
		}
	}
}
