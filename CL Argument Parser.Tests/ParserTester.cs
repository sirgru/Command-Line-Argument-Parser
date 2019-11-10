using NUnit.Framework;

namespace CLAP.Tests.ParserTesterNS
{
	[TestFixture]
	public class ParserTester
	{
		[Test]
		public void PathsOnly() {
			var parser = new Parser();
			var result = parser.Parse(new string[] {"path1", "path2"});

			Assert.AreEqual(2, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual("path2", result.paths[1]);
			Assert.AreEqual(0, result.switchesCount);
		}

		[Test]
		public void SwitchesOnly()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] {"-a", "--be"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(2, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual("be", result.switches[1].primaryName);
		}

		[Test]
		public void SwitchesAndPaths()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] {"-a", "--be", "path1"});

			Assert.AreEqual(1, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual(2, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual("be", result.switches[1].primaryName);
		}

		[Test]
		public void PathsThenSwitches_MeansPaths()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] { "path1", "-a", "--be", });

			Assert.AreEqual(3, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual("-a", result.paths[1]);
			Assert.AreEqual("--be", result.paths[2]);
		}

		[Test]
		public void AfterPathsDashMeansPaths()
		{
			var parser = new Parser();

			var result = parser.Parse(new string[] { "path1", "--", "path2", });

			Assert.AreEqual(3, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual("--", result.paths[1]);
			Assert.AreEqual("path2", result.paths[2]);
		}

		[Test]
		public void PathsMixedWithSwitches_MeansPaths()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] { "-a", "path1", "--be", });

			Assert.AreEqual(2, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual("--be", result.paths[1]);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
		}

		[Test]
		public void SwitchesWithArguments_OneOrNone()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			swA.AddParameter(Arity.NoneOrOne, "ones");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] {"-a=111", "--be"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(2, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(1, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
			Assert.AreEqual("be", result.switches[1].primaryName);
		}

		[Test]
		public void SwitchesWithArguments_OneOrNone_NoValue()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			swA.AddParameter(Arity.NoneOrOne, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(0, result.switches[0].argsCount);
		}

		[Test]
		public void SwitchesWithArguments_Wrong()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			swA.AddParameter(Arity.NoneOrOne, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "=", "111" });

			Assert.AreEqual(2, result.pathsCount);
			Assert.AreEqual("=", result.paths[0]);
			Assert.AreEqual("111", result.paths[1]);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(0, result.switches[0].argsCount);
		}

		[Test]
		public void SwitchesWithArguments_One()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			swA.AddParameter(Arity.One, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "111"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(1, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
		}

		[Test]
		public void SwitchesWithArguments_Many()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			swA.AddParameter(Arity.Many, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "111", "222"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(2, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
			Assert.AreEqual("222", result.switches[0].args[1]);
		}

		[Test]
		public void SwitchesWithArguments_ManyWithDash()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			swA.AddParameter(Arity.Many, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "111", "222", "--", "333" });

			Assert.AreEqual(1, result.pathsCount);
			Assert.AreEqual("333", result.paths[0]);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(2, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
			Assert.AreEqual("222", result.switches[0].args[1]);
		}

		[Test]
		public void SwitchesWithArguments_Many_InterruptedBySwitch()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("a");
			var swBE = new CommandSwitch("be");
			swA.AddParameter(Arity.Many, "ones");
			swBE.AddParameter(Arity.NoneOrOne, "threes");
			parser.AddSwitch(swA);
			parser.AddSwitch(swBE);

			var result = parser.Parse(new string[] {"-a", "111", "222", "--be=333"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(2, result.switchesCount);
			Assert.AreEqual("a", result.switches[0].primaryName);
			Assert.AreEqual(2, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
			Assert.AreEqual("222", result.switches[0].args[1]);
			Assert.AreEqual("be", result.switches[1].primaryName);
			Assert.AreEqual("333", result.switches[1].args[0]);
		}

		[Test]
		public void RepeatedSwitches_NoArg()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("AA", 'a');
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "--AA"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("AA", result.switches[0].primaryName);
			Assert.AreEqual(0, result.switches[0].argsCount);
		}

		[Test]
		public void RepeatedSwitches_OneArg()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("AA", 'a');
			swA.AddParameter(Arity.One, "ones");
			parser.AddSwitch(swA);

			var result = parser.Parse(new string[] {"-a", "111", "--AA"});

			Assert.AreEqual(0, result.pathsCount);
			Assert.AreEqual(1, result.switchesCount);
			Assert.AreEqual("AA", result.switches[0].primaryName);
			Assert.AreEqual(1, result.switches[0].argsCount);
			Assert.AreEqual("111", result.switches[0].args[0]);
		}

		[Test]
		public void RepeatedSwitches_OneArgIncorrect_Throws()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("AA", 'a');
			swA.AddParameter(Arity.One, "ones");
			parser.AddSwitch(swA);

			Assert.Throws<InvalidInput>(() => parser.Parse(new string[] {"-a", "111", "--AA", "111"}));
		}

		[Test]
		public void RepeatedSwitches_OneOrNoneArgIncorrect_Throws()
		{
			var parser = new Parser();
			var swA = new CommandSwitch("AA", 'a');
			swA.AddParameter(Arity.NoneOrOne, "ones");
			parser.AddSwitch(swA);

			Assert.Throws<InvalidInput>(() => parser.Parse(new string[] { "-a=111", "--AA=222" }));
		}

		[Test]
		public void Example_Documentation()
		{
			var swA = new CommandSwitch("AAA", 'a');
			swA.AddParameter(Arity.One, "a-param");
			swA.SetHelp("Help for A.");

			var swB = new CommandSwitch("be");
			swB.AddParameter(Arity.NoneOrOne, "b-param");
			swB.SetHelp("Help for B.");

			var parser = new Parser();
			parser.AddSwitch(swA);
			parser.AddSwitch(swB);

			var result = parser.Parse(new string[] {"-a", "111", "--be=222", "path1"});

			Assert.AreEqual(1, result.pathsCount);
			Assert.AreEqual("path1", result.paths[0]);
			Assert.AreEqual(2, result.switchesCount);
			Assert.AreEqual("AAA", result.switches[0].primaryName);
			Assert.AreEqual("111", result.switches[0].args[0]);
			Assert.AreEqual("be", result.switches[1].primaryName);
			Assert.AreEqual("222", result.switches[1].args[0]);
		}

	}
}
