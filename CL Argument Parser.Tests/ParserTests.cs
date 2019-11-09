using NUnit.Framework;

namespace CL_Argument_Parser.Tests.ParserTestsNS
{
	[TestFixture]
    public class ParserTests
    {
		[Test]
		public void Parse_DoubleDash()
		{
			var setup = new Setup(useDash: false, useDoubleDash: true);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"path1", "path2", "--switch", "switch-arg" });

			Assert.IsTrue(result.Paths.Count == 2);
			Assert.IsTrue(result.Paths[0] == "path1");
			Assert.IsTrue(result.Paths[1] == "path2");
			Assert.IsTrue(result.Switches.Count == 1);
			Assert.IsTrue(result.Switches[0].name == "switch");
			Assert.IsTrue(result.Switches[0].arguments.Count == 1);
			Assert.IsTrue(result.Switches[0].arguments[0] == "switch-arg");
		}

		[Test]
		public void Parse_SingleDash()
		{
			var setup = new Setup(useDash: true, useDoubleDash: false);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"path1", "path2", "-sw", "switch-arg" });

			Assert.IsTrue(result.Paths.Count == 2);
			Assert.IsTrue(result.Paths[0] == "path1");
			Assert.IsTrue(result.Paths[1] == "path2");
			Assert.IsTrue(result.Switches.Count == 2);
			Assert.IsTrue(result.Switches[0].name == "s");
			Assert.IsTrue(result.Switches[1].name == "w");
			Assert.IsTrue(result.Switches[0].arguments.Count == 0);
			Assert.IsTrue(result.Switches[1].arguments.Count == 1);
			Assert.IsTrue(result.Switches[1].arguments[0] == "switch-arg");
		}

		[Test]
		public void Parse_Slash()
		{
			var setup = new Setup(useDash: false, useDoubleDash: false, useSlash: true);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"/ab", "path" });

			Assert.IsTrue(result.Paths.Count == 0);
			Assert.IsTrue(result.Switches.Count == 1);
			Assert.IsTrue(result.Switches[0].name == "ab");
			Assert.IsTrue(result.Switches[0].arguments.Count == 1);
			Assert.IsTrue(result.Switches[0].arguments[0] == "path");
		}
	}
}