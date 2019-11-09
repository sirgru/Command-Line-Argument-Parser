namespace CLAP
{
	public class Parser
	{
		private Setup _setup;

		public Parser(Setup setup)
		{
			_setup = setup;
		}

		public ParseResult Parse(string[] args)
		{
			ParseResult result = new ParseResult();
			Switch lastSwitch = null;
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
					foreach (char c in value) {
						lastSwitch = result.TryAddSwitch(c.ToString());
					}
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
