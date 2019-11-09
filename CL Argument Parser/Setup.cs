namespace CL_Argument_Parser
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

		public Setup(bool useDash = true, bool useDoubleDash = true, bool useSlash = false)
		{
			this.useDash = useDash;
			this.useDoubleDash = useDoubleDash;
			this.useSlash = useSlash;
		}
	}
}
