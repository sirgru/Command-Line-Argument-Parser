using NUnit.Framework;

namespace CLAP.Tests.ParserSubTestsNS
{
	[TestFixture]
	public class ParserSubTests
	{
		[Test]
		public void GetType_OnPath()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("path");
			Assert.AreEqual(Parser.ArgType.Path, type);
			Assert.AreEqual("path", value);
			Assert.AreEqual(null, arg);
		}

		[Test]
		public void GetType_OnPathWithEquals_Accepts()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("path=other");
			Assert.AreEqual(Parser.ArgType.Path, type);
			Assert.AreEqual("path=other", value);
			Assert.AreEqual(null, arg);
		}

		[Test]
		public void GetType_OnSingleCharArg()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("-ab");
			Assert.AreEqual(Parser.ArgType.SingleCharSwitch, type);
			Assert.AreEqual("ab", value);
			Assert.AreEqual(null, arg);
		}

		[Test]
		public void GetType_OnSingleCharArgWArg()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("-ab=123");
			Assert.AreEqual(Parser.ArgType.SingleCharSwitch, type);
			Assert.AreEqual("ab", value);
			Assert.AreEqual("123", arg);
		}

		[Test]
		public void GetType_OnSingleCharArgWImproperArg_Throws()
		{
			var parser = new Parser();
			Assert.Throws<InvalidInput>(() => parser.GetArgType("-=123"));
		}

		[Test]
		public void GetType_OnSingleCharArgWImproperArg2_Throws()
		{
			var parser = new Parser();
			Assert.Throws<InvalidInput>(() => parser.GetArgType("-ab="));
		}

		[Test]
		public void GetType_OnStringArg()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("--argo");
			Assert.AreEqual(Parser.ArgType.StringSwitch, type);
			Assert.AreEqual("argo", value);
			Assert.AreEqual(null, arg);
		}

		[Test]
		public void GetType_OnStringArgWArg_Accepts()
		{
			var parser = new Parser();
			var (type, value, arg) = parser.GetArgType("--argo=111");
			Assert.AreEqual(Parser.ArgType.StringSwitch, type);
			Assert.AreEqual("argo", value);
			Assert.AreEqual("111", arg);
		}

		[Test]
		public void GetType_OnStringArgWbadArg_Throws()
		{
			var parser = new Parser();
			Assert.Throws<InvalidInput>(() => parser.GetArgType("--ab="));
		}

		[Test]
		public void GetType_OnStringArgWbadArg2_Throws()
		{
			var parser = new Parser();
			Assert.Throws<InvalidInput>(() => parser.GetArgType("--=111"));
		}
	}
}
