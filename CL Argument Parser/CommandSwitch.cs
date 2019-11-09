using System.Collections.Generic;
using System.Text;

namespace CLAP
{
	public class CommandSwitch
	{
		private string			_primaryName;
		private HashSet<string> _alternativeNames;
		private bool			_acceptsArguments;
		private string			_help;

		public string primaryName => _primaryName;
		public bool acceptsArguments => _acceptsArguments;

		public CommandSwitch(string primaryName, bool acceptsArguments)
		{
			_primaryName = primaryName;
			_acceptsArguments = acceptsArguments;
		}

		public void AddAlternativeName(string altName)
		{
			if (_alternativeNames == null) _alternativeNames = new HashSet<string>();
			_alternativeNames.Add(altName);
		}

		public void AddAlternativeNames(IEnumerable<string> names)
		{
			if (_alternativeNames == null) _alternativeNames = new HashSet<string>();
			foreach (var name in names) {
				_alternativeNames.Add(name);
			}
		}

		public void AddHelp(string help)
		{
			_help = help;
		}

		public bool IsIdentifiedBy(string other)
		{
			return _primaryName == other || _alternativeNames.Contains(other); 
		}

		public void GetHelp(Setup setup, StringBuilder sb)
		{
			int startingLength = sb.Length;
			sb.Append(' ', setup.leftMargin);
			AddNameWithDashes(_primaryName, setup, sb);
			if (_alternativeNames != null) {
				foreach (var altName in _alternativeNames) {
					sb.Append(", ");
					AddNameWithDashes(altName, setup, sb);
				}
			}
			// When names are wider than namesWidth, 
			// Overflow help text to a new line, from that width
			if (sb.Length - startingLength >= setup.namesWidth) {
				sb.Append('\n');
				sb.Append(' ', setup.namesWidth);
			} else {
				sb.Append(' ', setup.namesWidth - (sb.Length - startingLength));
			}

			sb.Append(_help).Append('\n');
		}

		private void AddNameWithDashes(string name, Setup setup, StringBuilder sb)
		{
			if (name.Length == 1) {
				if (setup.useDash) {
					sb.Append('-').Append(name);
					return;
				}
			}
			if (setup.useDoubleDash) {
				sb.Append("--").Append(name);
				return;
			}
			if (setup.useDoubleDash) {
				sb.Append('/').Append(name);
				return;
			}
			throw new Termination("Should never reach this.");
		}
	}
}
