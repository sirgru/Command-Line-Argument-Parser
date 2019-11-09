using System.Collections.Generic;

namespace CLAP
{
	/// <summary>
	/// Result of the operation of parsing
	/// </summary>
	public class CommandResult
	{
		public IReadOnlyList<string> paths { get; private set; }
		public IReadOnlyList<CommandSwitchResult> switches { get; private set; }

		public CommandResult(IReadOnlyList<string> paths, IReadOnlyDictionary<CommandSwitch, List<string>> switches)
		{
			var list = new List<CommandSwitchResult>();
			foreach (var sw in switches) {
				var (max, min) = sw.Key.GetOptionalityRange();
				if (sw.Value.Count > max || sw.Value.Count < min) {
					throw new InputException("Incorrect number of arguments for switch: " + sw.Key.primaryName);
				}
				list.Add(new CommandSwitchResult(sw.Key, sw.Value));
			}

			this.paths = paths;
			this.switches = list;
		}
	}

	/// <summary>
	/// Helper class specific for switches
	/// </summary>
	public class CommandSwitchResult
	{
		public readonly CommandSwitch commandSwitch;
		public readonly List<(string paramName, string argName)> arguments;
		public int argumentsCount => arguments?.Count ?? 0;

		public CommandSwitchResult(CommandSwitch commandSwitch, List<string> suppliedArguments)
		{
			this.commandSwitch = commandSwitch;

			if (suppliedArguments != null && suppliedArguments.Count > 0) {
				arguments = new List<(string paramName, string argName)>();
				for (int i = 0; i < suppliedArguments.Count; i++) {
					arguments.Add((commandSwitch.paramNames[i], suppliedArguments[i]));
				}
			}
		}
	}
}
