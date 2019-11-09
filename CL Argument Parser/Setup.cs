namespace CLAP
{
	public class Setup
    {
		/// <summary>
		/// Use a single dash ('-') for single-letter switches.
		/// </summary>
		public readonly bool useDash;

		/// <summary>
		/// Use a double dash ('--') for multi-letter switches.
		/// </summary>
		public readonly bool useDoubleDash;

		/// <summary>
		/// Use a single slash ('/') for multi-letter switches.
		/// Warning: this can work only on Windows 
		/// as on UNIX and like it is a valid path.
		/// </summary>
		public readonly bool useSlash;

		/// <summary>
		/// Width reserved for names when displaying help
		/// </summary>
		public readonly int namesWidth;

		/// <summary>
		/// Width of the left margin when displaying help.
		/// </summary>
		public readonly int leftMargin;

		public Setup(bool useDash = true, bool useDoubleDash = true, bool useSlash = false, int namesWidth = 26, int leftMargin = 4)
		{
			this.useDash = useDash;
			this.useDoubleDash = useDoubleDash;
			this.useSlash = useSlash;

			if (namesWidth < 1) throw new Termination("Invalid value for namesWidth: must be greater than 0.");
			this.namesWidth = namesWidth;

			if (leftMargin < 1) throw new Termination("Invalid value for leftMargin: must be greater than 0.");
			this.leftMargin = leftMargin;
		}
	}
}
