using System.Collections.Generic;

namespace CL_Argument_Parser
{
	public class Parser
	{
		private Setup _setup;

		public Parser()
		{
			_setup = new Setup();
		}

		public Parser(Setup setup)
		{
			_setup = setup;
		}

		public Result Parse(string[] args)
		{
			Result result = new Result();
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
						lastSwitch = new Switch(c.ToString());
						result.switches.Add(lastSwitch);
					}
					break;
				case SwitchType.Multi:
					lastSwitch = new Switch(value);
					result.switches.Add(lastSwitch);
					break;
				default:
					break;
				}
			}
			return result;
		}

		enum SwitchType {
			None, Single, Multi
		}

		private (SwitchType type, string value) SwitchOrNot(string arg)
		{
			if (_setup.useDash && arg.StartsWith('-')) return (SwitchType.Single, arg.Substring(1));
			if (_setup.useDoubleDash && arg.StartsWith("--")) return (SwitchType.Multi, arg.Substring(2));
			if (_setup.useSlash && arg.StartsWith("/")) return (SwitchType.Multi, arg.Substring(1));
			return (SwitchType.None, arg);
		}
	}

	public class Result
	{
		internal List<string> paths = new List<string>();
		internal List<Switch> switches = new List<Switch>();

		public IReadOnlyList<string> Paths => paths;
		public IReadOnlyList<Switch> Switches => switches;
	}
}
