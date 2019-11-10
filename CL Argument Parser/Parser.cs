using System;
using System.Collections.Generic;
using System.Text;

namespace CLAP
{
	/// <summary>
	/// Main class providing the parsing
	/// </summary>
	public class Parser
	{
		private List<CommandSwitch> _switches = new List<CommandSwitch>();

		/// <summary>
		/// Registers the unique switch with the parser.
		/// </summary>
		/// <exception cref="ArgumentException">Switch cannot be null.</exception>
		public void AddSwitch(CommandSwitch sw)
		{
			if (sw == null) throw new ArgumentException("Switch cannot be null.");
			_switches.Add(sw);
		}

		enum PathSplit { Not, NotForOne, Maybe, Yes }

		/// <summary>
		/// Main parsing method
		/// </summary>
		/// <exception cref="Termination">Internal error</exception>
		/// <exception cref="InvalidInput">Input was not valid according to parsing rules</exception>
		public ParseResult Parse(string[] args)
		{
			var result = new ParseResult.Builder();
			PathSplit psp = PathSplit.Maybe;

			foreach (var arg in args) {
				switch (psp) {
				case PathSplit.Not:
				case PathSplit.Maybe: {
					var (argType, value, swArg) = GetArgType(arg);

					if (argType == ArgType.Separator) {
						psp = PathSplit.Yes;
						break;
					}
					else if (argType == ArgType.SingleCharSwitch) {
						AddSingleCharSwitch(ref psp, value, swArg, result);
						break;
					}
					else if (argType == ArgType.StringSwitch) {
						AddStringSwitch(ref psp, value, swArg, result);
						break;
					}
					else if (argType == ArgType.Path) {
						if (psp == PathSplit.Not) {
							result.AddArgForLastSwitch(value);
							// psp remains .Not.
							break;
						}
						else if (psp == PathSplit.Maybe) {
							result.AddPath(value);
							psp = PathSplit.Yes;
							break;
						}
						throw new Termination("Should never reach this.");
					}
					throw new Termination("Should never reach this.");
				}
				case PathSplit.NotForOne: {
					var (argType, value, swArg) = GetArgType(arg);

					if (argType != ArgType.Path) throw new InvalidInput("Expected path but got a switch at " + arg);
					var lastSwitch = result.GetLastSwitch();
					if (lastSwitch.arity == Arity.One && result.AlreadyHasArgumentForLastSwitch()) {
						throw new InvalidInput($"Switch {value} can accept only one argument, but two are supplied.");
					}

					psp = PathSplit.Maybe;
					result.AddArgForLastSwitch(arg);
					break;
				}
				case PathSplit.Yes: {
					result.AddPath(arg);
					break;
				}
				default:  throw new Termination("Should never reach this.");
				}
			}
			return result.Build();
		}

		private void AddSingleCharSwitch(ref PathSplit psp, string value, string arg, ParseResult.Builder result)
		{
			// Add all but last with null arg
			for (int i = 0; i < value.Length - 1; i++) {
				AddStringSwitch(ref psp, value[0].ToString(), null, result);
			}
			// Add last with actual arg
			AddStringSwitch(ref psp, value[value.Length - 1].ToString(), arg, result);
		}

		private void AddStringSwitch(ref PathSplit psp, string value, string arg, ParseResult.Builder result)
		{
			CommandSwitch sw = FindSwitchIdentifiedBy(value);
			if (sw == null) {
				throw new InvalidInput("Invalid switch: " + value);
			}
			if (arg != null) {
				if (sw.arity != Arity.NoneOrOne) {
					throw new InvalidInput($"Switch {value} has argument, which is not allowed for the given switch.");
				}
				result.AddSwitch(sw);

				if ((sw.arity == Arity.One || sw.arity == Arity.NoneOrOne) && result.AlreadyHasArgumentForLastSwitch()) {
					throw new InvalidInput($"Switch {value} can accept only one argument, but two are supplied.");
				}
				result.AddArgForLastSwitch(arg);
				psp = PathSplit.Maybe;
			}
			else {
				switch (sw.arity) {
				case Arity.None:		psp = PathSplit.Maybe; break;
				case Arity.One:			psp = PathSplit.NotForOne; break;
				case Arity.NoneOrOne:	psp = PathSplit.Maybe; break;
				case Arity.Many:		psp = PathSplit.Not; break;
				default: throw new Termination("Should not reach this.");
				}
				result.AddSwitch(sw);
			}
		}

		internal enum ArgType
		{
			Path, SingleCharSwitch, StringSwitch, Separator
		}

		internal (ArgType type, string value, string argument) GetArgType(string arg)
		{
			if (arg.StartsWith('-') && !arg.StartsWith("--")) {
				var t = ArgType.SingleCharSwitch;
				var split = SeparateArgument(arg.Substring(1));
				return (t, split.name, split.argument);
			}
			if (arg.StartsWith("--")) {
				if (arg == "--") return (ArgType.Separator, null, null);
				else {
					var t = ArgType.StringSwitch;
					var split = SeparateArgument(arg.Substring(2));
					return (t, split.name, split.argument);
				}
			}
			return (ArgType.Path, arg, null);
		}

		private (string name, string argument) SeparateArgument(string input)
		{
			if (input.StartsWith('=')) throw new InvalidInput("Invalid switch");

			var index = input.IndexOf('=');
			if (index == -1) return (input, null);
			else {
				if (index == input.Length - 1) throw new InvalidInput("Invalid switch");
				return (input.Substring(0, index), input.Substring(index + 1));
			}
		}

		private CommandSwitch FindSwitchIdentifiedBy(string id)
		{
			foreach (var sw in _switches) {
				if (sw.IsIdentifiedBy(id)) return sw;
			}
			return null;
		}

		public void GetHelp(Setup setup, StringBuilder builder, bool onlyImportant = false)
		{
			foreach (var sw in _switches) {
				sw.GetHelp(setup, builder, onlyImportant);
			}
		}
	}
}
