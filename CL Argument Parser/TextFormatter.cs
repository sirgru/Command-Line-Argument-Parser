using System;
using System.Text;

namespace CLAP
{
	public static class TextFormatter
	{
		/// <summary>
		/// Main formatting funciton.
		/// </summary>
		/// <param name="input">Text to be formatted.</param>
		/// <param name="lineWidth">The width of the line including margins.</param>
		/// <param name="leftMargin">The width of the margin.</param>
		/// <param name="firstLineLeftMargin">The width of the margin for first line.</param>
		/// <param name="firstLineWidth">If greater than 0, first line will be of specified width.</param>
		/// <returns>Formatted input</returns>
		/// <exception cref="ArgumentException"></exception>
		public static (string result, bool hasMoreThanOneLine) Format(string input, int lineWidth, int leftMargin, int firstLineLeftMargin = 0, int firstLineWidth = 0, bool justify = false)
		{
			if (input == null) throw new ArgumentException("Input cannot be null");
			if (lineWidth < 4) throw new ArgumentException("Line must be wider than 3.");
			if (firstLineLeftMargin >= lineWidth || leftMargin >= lineWidth ) throw new ArgumentException("Line width must be greater than margins.");

			if (firstLineWidth == 0) firstLineWidth = lineWidth;
			if (input.Length + firstLineLeftMargin <= firstLineWidth) return (new String(' ', firstLineLeftMargin) + input, false);

			StringBuilder sbLine = new StringBuilder(lineWidth);
			StringBuilder sbTotal = new StringBuilder();
			StringBuilder sbTemp = new StringBuilder(lineWidth);

			AddLeftMargin(sbLine, firstLineLeftMargin);
			bool isFirstLine = true;

			for (int i = 0, lastWhitespaceIndex = -1; i < input.Length; i++) {
				var c = input[i];
				sbLine.Append(c);

				if (c == '\n') {
					PourEverything(sbLine, sbTotal);
					AddLeftMargin(sbLine, leftMargin);
					lastWhitespaceIndex = -1;
					isFirstLine = false;
					continue;
				}

				if (char.IsWhiteSpace(c)) lastWhitespaceIndex = sbLine.Length - 1;
				if ((sbLine.Length == lineWidth && !isFirstLine) || (sbLine.Length == firstLineWidth && isFirstLine)) {
					Pour(sbLine, sbTotal, sbTemp, isFirstLine? firstLineWidth : lineWidth, lastWhitespaceIndex, leftMargin, isFirstLine? firstLineLeftMargin : leftMargin, justify);
					lastWhitespaceIndex = -1;
					isFirstLine = false;
				}
			}

			// Last line
			if (sbLine.Length > 0) {
				sbTotal.Append(sbLine);
			}

			return (sbTotal.ToString(), !isFirstLine);
		}

		private static void AddLeftMargin(StringBuilder sb, int width)
		{
			sb.Append(' ', width);
		}

		private static void PourEverything(StringBuilder from, StringBuilder to)
		{
			to.Append(from);
			from.Clear();
		}

		private static void Pour(StringBuilder from, StringBuilder to, StringBuilder temp, int lineWidth, int lastWhitespaceIndex, int leftMargin, int currentLineMargin, bool justify)
		{
			if (lastWhitespaceIndex < 1) {
				to.Append(from);
				from.Clear();
				to.Append('\n');
				AddLeftMargin(from, leftMargin);
			}
			else {

				if (justify) {
					for (int i = 0; i <= lastWhitespaceIndex; i++) temp.Append(from[i]);
					Justify(temp, lastWhitespaceIndex, lineWidth, currentLineMargin);
					to.Append(temp);
					temp.Clear();
				}
				else {
					for (int i = 0; i <= lastWhitespaceIndex; i++) to.Append(from[i]);
				}

				// for (int i = lastWhitespaceIndex + 1; i < from.Length; i++) to.Append('_');
				if (from[lastWhitespaceIndex] != '\n' && from[lastWhitespaceIndex] != '\r') to.Append('\n');
				AddLeftMargin(temp, leftMargin);
				for (int i = lastWhitespaceIndex + 1; i < from.Length; i++) temp.Append(from[i]);
				from.Clear();
				from.Append(temp);
				temp.Clear();
			}
		}

		private static void Justify(StringBuilder temp, int lastWhitespaceIndex, int lineWidth, int currentLineMargin)
		{
			while (temp[temp.Length - 1] == ' ') temp.Length--;
			while (temp.Length < lineWidth) {
				for (int i = currentLineMargin; i < temp.Length && temp.Length < lineWidth; i++) {
					if (char.IsWhiteSpace(temp[i])) {
						temp.Insert(i, ' ');
						i++;
					}
				}
			}
		}

		public enum OutlineType { Line, Equals, Underline }

		public static string FormatHeader(string input, int headerWidth, OutlineType outlineType, int leftMargin = 0)
		{
			if (leftMargin < 0) throw new ArgumentException("Left margin cannot be less than zero.");
			if (input.Length > headerWidth) throw new ArgumentException("Header cannot be wider than the line.");

			StringBuilder sb = new StringBuilder();
			sb.Append(' ', leftMargin);
			var length = headerWidth / 2 - input.Length / 2 - 1;

			var c = (outlineType == OutlineType.Line)? '-' : (outlineType == OutlineType.Equals) ? '=' : ' ';
			for (int i = 0; i < length; i++) sb.Append(c);
			sb.Append(' ');
			sb.Append(input);
			sb.Append(' ');
			for (int i = 0; i < length; i++) sb.Append(c);

			if (outlineType == OutlineType.Underline) {
				sb.Append('\n');
				sb.Append(' ', leftMargin);
				for (int i = 0; i < length + 1; i++) sb.Append(' ');
				for (int i = 0; i < input.Length; i++) sb.Append('-');
			}

			return sb.ToString();
		}
	}
}
