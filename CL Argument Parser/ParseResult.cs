using System.Collections.Generic;

namespace CLAP
{
	public class ParseResult
	{
		public IReadOnlyList<string> paths { get; private set; }
		public IReadOnlyList<ParseResultSwitch> switches { get; private set; }
		public int pathsCount => paths?.Count ?? 0; 
		public int switchesCount => switches?.Count ?? 0;

		private ParseResult() { }

		internal class Builder
		{
			private List<string> _paths;
			private List<ParseResultSwitch.Builder> _switches;

			private ParseResultSwitch.Builder _last;

			internal void AddPath(string path)
			{
				if (_paths == null) _paths = new List<string>();
				_paths.Add(path);
			}

			internal void AddSwitch(CommandSwitch csw)
			{
				if (_switches == null) _switches = new List<ParseResultSwitch.Builder>();
				var found = _switches.Find(x => x.csw == csw);
				if (found == null) {
					_last = new ParseResultSwitch.Builder(csw);
					_switches.Add(_last);
				} else {
					_last = found;
				}
			}

			internal void AddArgForLastSwitch(string arg)
			{
				_last.AddArg(arg);
			}

			internal bool AlreadyHasArgumentForLastSwitch()
			{
				return _last.HasArgs();
			}

			internal ParseResult Build()
			{
				var result = new ParseResult();
				result.paths = _paths;
				result.switches = BuildSwitches();
				return result;
			}

			internal CommandSwitch GetLastSwitch()
			{
				return _last?.csw;
			}

			internal List<ParseResultSwitch> BuildSwitches()
			{
				if (_switches == null) return null;
				var result = new List<ParseResultSwitch>();
				foreach (var sw in _switches) {
					result.Add(sw.Build());
				}
				return result;
			}
		}

	}

	public class ParseResultSwitch
	{
		public CommandSwitch commandSwitch { get; private set; }
		public IReadOnlyList<string> args { get; private set; }
		public int argsCount => args?.Count ?? 0;
		public string primaryName => commandSwitch.primaryName;

		private ParseResultSwitch() { }

		internal class Builder
		{
			internal CommandSwitch csw;
			private List<string> _args;
			internal Builder(CommandSwitch csw)
			{
				this.csw = csw;
			}

			internal void AddArg(string arg)
			{
				if (_args == null) _args = new List<string>();
				_args.Add(arg);
			}

			internal bool HasArgs()
			{
				return _args != null;
			}

			internal ParseResultSwitch Build()
			{
				var result = new ParseResultSwitch();
				result.commandSwitch = csw;
				result.args = _args;
				return result;
			}
		}

	}



	///// <summary>
	///// Result of the operation of parsing
	///// </summary>
	//public class CommandResult
	//{
	//	public IReadOnlyList<string> paths { get; private set; }
	//	public IReadOnlyList<CommandSwitchResult> switches { get; private set; }

	//	public CommandResult(IReadOnlyList<string> paths, IReadOnlyDictionary<CommandSwitch, List<string>> switches)
	//	{
	//		var list = new List<CommandSwitchResult>();
	//		foreach (var sw in switches) {
	//			var (max, min) = sw.Key.GetOptionalityRange();
	//			if (sw.Value.Count > max || sw.Value.Count < min) {
	//				throw new InputException("Incorrect number of arguments for switch: " + sw.Key.primaryName);
	//			}
	//			list.Add(new CommandSwitchResult(sw.Key, sw.Value));
	//		}

	//		this.paths = paths;
	//		this.switches = list;
	//	}
	//}

	///// <summary>
	///// Helper class specific for switches
	///// </summary>
	//public class CommandSwitchResult
	//{
	//	public readonly CommandSwitch commandSwitch;
	//	public readonly List<(string paramName, string argName)> arguments;
	//	public int argumentsCount => arguments?.Count ?? 0;

	//	public CommandSwitchResult(CommandSwitch commandSwitch, List<string> suppliedArguments)
	//	{
	//		this.commandSwitch = commandSwitch;

	//		if (suppliedArguments != null && suppliedArguments.Count > 0) {
	//			arguments = new List<(string paramName, string argName)>();
	//			for (int i = 0; i < suppliedArguments.Count; i++) {
	//				arguments.Add((commandSwitch.paramNames[i], suppliedArguments[i]));
	//			}
	//		}
	//	}
	//}
}
