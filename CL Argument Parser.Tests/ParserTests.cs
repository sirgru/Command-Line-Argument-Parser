using NUnit.Framework;

namespace CLAP.Tests.ParserTestsNS
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

			Assert.AreEqual(2, result.Paths.Count);
			Assert.AreEqual("path1", result.Paths[0]);
			Assert.AreEqual("path2", result.Paths[1]);
			Assert.AreEqual(1, result.Switches.Count);
			Assert.AreEqual("switch", result.Switches[0].name);
			Assert.AreEqual(1, result.Switches[0].argumentsCount);
			Assert.AreEqual("switch-arg", result.Switches[0].arguments[0]);
		}

		[Test]
		public void Parse_SingleDash()
		{
			var setup = new Setup(useDash: true, useDoubleDash: false);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"path1", "path2", "-sw", "switch-arg" });

			Assert.AreEqual(2, result.Paths.Count);
			Assert.AreEqual("path1", result.Paths[0]);
			Assert.AreEqual("path2", result.Paths[1]);
			Assert.AreEqual(2, result.Switches.Count);
			Assert.AreEqual("s", result.Switches[0].name);
			Assert.AreEqual("w", result.Switches[1].name);
			Assert.AreEqual(0, result.Switches[0].argumentsCount);
			Assert.AreEqual(1, result.Switches[1].argumentsCount);
			Assert.AreEqual("switch-arg", result.Switches[1].arguments[0]);
		}

		[Test]
		public void Parse_Slash()
		{
			var setup = new Setup(useDash: false, useDoubleDash: false, useSlash: true);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"/ab", "path" });

			Assert.AreEqual(0, result.Paths.Count);
			Assert.AreEqual(1, result.Switches.Count);
			Assert.AreEqual("ab", result.Switches[0].name);
			Assert.AreEqual(1, result.Switches[0].argumentsCount);
			Assert.AreEqual("path", result.Switches[0].arguments[0]);
		}

		[Test]
		public void Parse_AllSwitchTypes()
		{
			var setup = new Setup(useDash: true, useDoubleDash: true, useSlash: true);
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"/a", "-a", "-ab", "--abc" });

			Assert.AreEqual(0, result.Paths.Count);
			Assert.AreEqual(3, result.Switches.Count);
			Assert.AreEqual("a", result.Switches[0].name);
			Assert.AreEqual(0, result.Switches[0].argumentsCount);
			Assert.AreEqual("b", result.Switches[1].name);
			Assert.AreEqual(0, result.Switches[1].argumentsCount);
			Assert.AreEqual("abc", result.Switches[2].name);
			Assert.AreEqual(0, result.Switches[2].argumentsCount);
		}

		[Test]
		public void Parse_SwichArgumentAfterEq()
		{
			var setup = new Setup();
			var parser = new Parser(setup);
			var result = parser.Parse(new string[] {"-a=111", "--baz=222" });

			Assert.AreEqual(0, result.Paths.Count);
			Assert.AreEqual(2, result.Switches.Count);
			Assert.AreEqual("a", result.Switches[0].name);
			Assert.AreEqual(0, result.Switches[0].argumentsCount);
			Assert.AreEqual(1, result.Switches[0].tightArgumentsCount);
			Assert.AreEqual("111", result.Switches[0].tightArguments[0]);
			Assert.AreEqual("baz", result.Switches[1].name);
			Assert.AreEqual(0, result.Switches[1].argumentsCount);
			Assert.AreEqual(1, result.Switches[1].tightArgumentsCount);
			Assert.AreEqual("222", result.Switches[1].tightArguments[0]);
		}
	}
}
