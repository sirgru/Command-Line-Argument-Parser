namespace CLAP
{
	/// <summary>
	/// Class that transforms input text into intermeidate form
	/// </summary>
	public class Parser
	{
		private Setup _setup;

		public Parser(Setup setup)
		{
			_setup = setup;
		}

		/// <summary>
		/// Activation method
		/// </summary>
		/// <exception cref="Termination">Internal System Error</exception>
		/// <exception cref="InputException">Input was invalid</exception>
		public ParseResult Parse(string[] args)
		{
			ParseResult result = new ParseResult();
			TextSwitch lastSwitch = null;
			foreach (var arg in args) {
				var (type, value) = SwitchOrNot(arg);

				switch (type) {
				case SwitchType.None:
					if (lastSwitch == null) {
						result.paths.Add(arg);
					}
					else {
						lastSwitch.AddArgument(arg);
					}
					break;
				case SwitchType.Single:
					lastSwitch = result.TryAddManySwitches(value);
					break;
				case SwitchType.Multi:
					lastSwitch = result.TryAddSwitch(value);
					break;
				default: throw new Termination("Unhandled case.");
				}
			}
			return result;
		}

		enum SwitchType {
			None, Single, Multi
		}

		private (SwitchType type, string value) SwitchOrNot(string arg)
		{
			if (_setup.useDash && arg.StartsWith('-') && !(_setup.useDoubleDash && arg.StartsWith("--"))) return (SwitchType.Single, arg.Substring(1));
			if (_setup.useDoubleDash && arg.StartsWith("--")) return (SwitchType.Multi, arg.Substring(2));
			if (_setup.useSlash && arg.StartsWith("/")) return (SwitchType.Multi, arg.Substring(1));
			return (SwitchType.None, arg);
		}
	}
}
