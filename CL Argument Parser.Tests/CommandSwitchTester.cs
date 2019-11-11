using NUnit.Framework;
using System;
using System.Text;

namespace CLAP.Tests.CommandSwitchTesterNS
{
	[TestFixture]
    class CommandSwitchTester
    {
		[Test]
		public void MakeSwitch_Throws1()
		{
			Assert.Throws<ArgumentException>(() => new CommandSwitch("A", 'a'));
		}

		[Test]
		public void MakeSwitch_Throws2()
		{
			Assert.Throws<ArgumentException>(() => new CommandSwitch(null, 'a'));
		}

		[Test]
		public void MakeSwitch_Throws3()
		{
			Assert.Throws<ArgumentException>(() => new CommandSwitch(null));
		}

		[Test]
		public void MakeSwitch_Throws4()
		{
			Assert.Throws<ArgumentException>(() => new CommandSwitch(""));
		}

		[Test]
		public void MakeSwitch_SingleCharString()
		{
			var sw = new CommandSwitch("A");
			Assert.AreEqual("A", sw.primaryName);
		}

		[Test]
		public void MakeSwitch_WithShortName_GetsLong()
		{
			var sw = new CommandSwitch("AAA", 'a');
			Assert.AreEqual("AAA", sw.primaryName);
		}

		[Test]
		public void GetHelp_NoParam()
		{
			var sw1 = new CommandSwitch("switch", 's');
			sw1.AddAlternativeNames("alt");
			sw1.SetHelp("Help here");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = " -s --switch, --alt       Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_OneParam()
		{
			var sw1 = new CommandSwitch("switch", 's');
			sw1.AddAlternativeNames("alt");
			sw1.SetHelp("Help here");
			sw1.AddParameter(Arity.One, "p");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = " -s --switch, --alt <p>   Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_MaybeParam()
		{
			var sw1 = new CommandSwitch("switch");
			sw1.SetHelp("Help here");
			sw1.AddParameter(Arity.NoneOrOne, "p");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = "    --switch[=p]          Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_ManyParam()
		{
			var sw1 = new CommandSwitch("switch");
			sw1.SetHelp("Help here");
			sw1.AddParameter(Arity.Any, "p");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = "    --switch <p...>       Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_Long_Overflows()
		{
			var sw1 = new CommandSwitch("switch", 's');
			sw1.AddAlternativeNames("alternative");
			sw1.SetHelp("Help here");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = " -s --switch, --alternative\n                          Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_WhenNoLongNameAndNoAltNames()
		{
			var sw1 = new CommandSwitch("s");
			sw1.SetHelp("Help here");
			sw1.AddParameter(Arity.NoneOrOne, "p");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = " -s[=p]                   Help here\n";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetHelp_WithAltNamesNoLongname_CorrectDisplay()
		{
			var sw1 = new CommandSwitch("s");
			sw1.AddAlternativeNames("alternative");
			sw1.SetHelp("Help here");

			StringBuilder sb = new StringBuilder();
			sw1.GetHelp(new Setup(), sb);
			var actual = sb.ToString();
			var expected = " -s --alternative         Help here\n";
			Assert.AreEqual(expected, actual);
		}

		//[Test]
		//public void GetHelp_WithAltNamesNoLongname_CorrectDisplay()
		//{
		//	var sw1 = new CommandSwitch("s");
		//	sw1.AddAlternativeNames("alternative");
		//	sw1.SetHelp("Help here");

		//	StringBuilder sb = new StringBuilder();
		//	sw1.GetHelp(new Setup(), sb);
		//	var actual = sb.ToString();
		//	var expected = " -s --alternative         Help here\n";
		//	Assert.AreEqual(expected, actual);
		//}
	}
}
