using System.Collections.Generic;
using System.Text;

namespace CLAP
{
	public class Command
	{
		private static readonly IReadOnlyList<string> _empty = new List<string>();

		private List<CommandSwitch> _switches = new List<CommandSwitch>();

		private List<string> _paths = new List<string>();
		private MultiDictionary<CommandSwitch, string> _switchToArguments = new MultiDictionary<CommandSwitch, string>();

		private Setup _setup;

		public Command(Setup setup)
		{
			_setup = setup;
		}

		public void AddSwitch(CommandSwitch csw)
		{
			_switches.Add(csw);
		}

		public CommandResult Adapt(ParseResult parseResult)
		{
			foreach (var path in parseResult.paths) {
				_paths.Add(path);
			}
			foreach (var textSwitch in parseResult.switches) {
				var sw = GetCommandSwitch(textSwitch.name);
				if (sw == null) throw new InputException("Invalid Switch: " + textSwitch.name);

				if (sw.acceptsArguments) {
					_switchToArguments.AddRange(sw, textSwitch.arguments ?? _empty);
				}
				else {
					_paths.AddRange(textSwitch.arguments ?? _empty);
				}
			}
			return new CommandResult(_paths, _switchToArguments.backing);
		}

		private CommandSwitch GetCommandSwitch(string switchName)
		{
			return _switches.Find(x => x.IsIdentifiedBy(switchName));
		}

		public string GetHelp()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("All available switches:\n\n");

			foreach (var sw in _switches) {
				sw.GetHelp(_setup, sb);
			}
			return sb.ToString();
		}
	}

	public class CommandResult
	{
		public IReadOnlyList<string> paths { get; private set; }
		public IReadOnlyDictionary<CommandSwitch, List<string>> switches { get; private set; }

		public CommandResult(IReadOnlyList<string> paths, IReadOnlyDictionary<CommandSwitch, List<string>> switches)
		{
			this.paths = paths;
			this.switches = switches;
		}
	}
}
