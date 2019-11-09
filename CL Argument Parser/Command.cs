using System.Collections.Generic;
using System.Text;

namespace CLAP
{
	/// <summary>
	/// Contains logical information about commands
	/// </summary>
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

		/// <summary>
		/// Main parsing method
		/// </summary>
		/// <exception cref="Termination">Internal System Error</exception>
		/// <exception cref="InputException">Input was invalid</exception>
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
					_switchToArguments.AddRange(sw, textSwitch.tightArguments ?? _empty);
				}
				else {
					_paths.AddRange(textSwitch.arguments ?? _empty);
					if (textSwitch.tightArguments != null) throw new InputException("Switch " + textSwitch.name + " does not accept arguments.");
				}
			}
			return new CommandResult(_paths, _switchToArguments.backing);
		}

		private CommandSwitch GetCommandSwitch(string switchName)
		{
			return _switches.Find(x => x.IsIdentifiedBy(switchName));
		}

		public string GetHelp(string header = null)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(header ?? "All available switches:\n\n");

			foreach (var sw in _switches) {
				sw.GetHelp(_setup, sb);
			}
			return sb.ToString();
		}
	}
}
