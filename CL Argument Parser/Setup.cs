using System;

namespace CLAP
{
	public class Setup
    {
		/// <summary>
		/// Width reserved for names when displaying help
		/// </summary>
		public readonly int namesWidth;

		/// <summary>
		/// Width of the left margin when displaying help.
		/// </summary>
		public readonly int leftMargin;

		/// <summary>
		/// Configuration object for CLAP
		/// </summary>
		/// <param name="namesWidth">Width reserved for names when displaying help</param>
		/// <param name="leftMargin">Width of the left margin when displaying help.</param>
		/// <exception cref="ArgumentException"></exception>
		public Setup(int namesWidth = 26, int leftMargin = 1)
		{
			if (namesWidth < 1) throw new ArgumentException("Invalid value for namesWidth: must be greater than 0.");
			this.namesWidth = namesWidth;

			if (leftMargin < 0) throw new ArgumentException("Invalid value for leftMargin: must be greater than or equal to 0.");
			this.leftMargin = leftMargin;
		}
	}
}
