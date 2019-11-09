using System.Collections.Generic;

namespace CLAP
{
	public class CommandResult
	{
		public IReadOnlyList<string> paths { get; private set; }
		public IReadOnlyDictionary<CommandSwitch, List<string>> switches { get; private set; }

		public CommandResult(IReadOnlyList<string> paths, IReadOnlyDictionary<CommandSwitch, List<string>> switches)
		{
			foreach (var sw in switches) {
				var (max, min) = sw.Key.GetOptionalityRange();
				if (sw.Value.Count > max || sw.Value.Count < min) {
					throw new InputException("Incorrect number of arguments for switch: " + sw.Key.primaryName);
				}
			}

			this.paths = paths;
			this.switches = switches;
		}
	}
}
