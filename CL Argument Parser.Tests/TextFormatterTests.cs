using NUnit.Framework;

namespace CLAP.Tests.TextFormatterTestsNS
{
	[TestFixture]
	public class TextFormatterTests
	{
		[Test]
		public void Justified()
		{
			var text = "Lorem ipsum dolor sit amet, consecteturoriumuselratietoneritumusaden adipiscing elit. Aenean nec convallis magna, in pretium tellus.\n\nPellentesque mattis arcu sed neque pretium tincidunt. Nam magna neque, convallis quis dapibus sit amet, pulvinar sit amet arcu.";
			var actual = TextFormatter.Format(input: text, lineWidth: 40, leftMargin: 2, firstLineLeftMargin: 4, justify: true);
			var expected = @"    Lorem    ipsum    dolor   sit  amet,
  consecteturoriumuselratietoneritumusad
  en    adipiscing    elit.  Aenean  nec
  convallis magna, in pretium tellus.
  
  Pellentesque  mattis  arcu  sed  neque
  pretium  tincidunt.  Nam  magna neque,
  convallis    quis  dapibus  sit  amet,
  pulvinar sit amet arcu.".Replace("\r\n", "\n");

			Assert.AreEqual(expected, actual.result);
		}

		[Test]
		public void NotJustified()
		{
			var text = "Lorem ipsum dolor sit amet, consecteturoriumuselratietoneritumusaden adipiscing elit. Aenean nec convallis magna, in pretium tellus.\n\nPellentesque mattis arcu sed neque pretium tincidunt. Nam magna neque, convallis quis dapibus sit amet, pulvinar sit amet arcu.";
			var actual = TextFormatter.Format(input: text, lineWidth: 40, leftMargin: 2, firstLineLeftMargin: 4, justify: false);
			var expected = @"    Lorem ipsum dolor sit amet, 
  consecteturoriumuselratietoneritumusad
  en adipiscing elit. Aenean nec 
  convallis magna, in pretium tellus.
  
  Pellentesque mattis arcu sed neque 
  pretium tincidunt. Nam magna neque, 
  convallis quis dapibus sit amet, 
  pulvinar sit amet arcu.".Replace("\r\n", "\n");

			Assert.AreEqual(expected, actual.result);
		}

		[Test]
		public void LineCounter1()
		{
			var text = "Lorem ipsum dolor sit amet";
			var actual = TextFormatter.Format(input: text, lineWidth: 40, leftMargin: 2);
			Assert.AreEqual(false, actual.hasMoreThanOneLine);
		}

		[Test]
		public void LineCounter2()
		{
			var text = "Lorem ipsum dolor sit amet, consecteturoriumuselratietoneritumusaden adipiscing elit. Aenean nec convallis magna, in pretium tellus.";
			var actual = TextFormatter.Format(input: text, lineWidth: 40, leftMargin: 2);
			Assert.AreEqual(true, actual.hasMoreThanOneLine);
		}

		[Test]
		public void Header_Line()
		{
			var actual = TextFormatter.FormatHeader("HEADER", 40, TextFormatter.OutlineType.Line, leftMargin: 10);
			var expected = "          ---------------- HEADER ----------------";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Header_Equals()
		{
			var actual = TextFormatter.FormatHeader("HEADER", 40, TextFormatter.OutlineType.Equals, leftMargin: 10);
			var expected = "          ================ HEADER ================";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Header_Underline()
		{
			var actual = TextFormatter.FormatHeader("HEADER", 12, TextFormatter.OutlineType.Underline, leftMargin: 5);
			var expected = "        HEADER   \n        ------";
			Assert.AreEqual(expected, actual);
		}

	}
}
