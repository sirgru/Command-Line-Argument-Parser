using System.Collections.Generic;
using System.Text;

namespace CLAP
{
	public class CommandSwitch
	{
		private string			_primaryName;
		private HashSet<string> _alternativeNames;
		private List<string>	_argumentNames = new List<string>();
		private List<bool>		_argumentOptionality = new List<bool>();
		private bool            _lastAddedArgumentWasOptional = false;
		private string			_help;

		public string primaryName => _primaryName;
		public bool acceptsArguments => _argumentNames?.Count > 0;
		public int argumentsCount => _argumentNames?.Count ?? 0;

		public CommandSwitch(string primaryName)
		{
			_primaryName = primaryName;
		}

		public void AddArgument(string argName, bool isOptional = false)
		{
			if (_lastAddedArgumentWasOptional && isOptional == false) {
				throw new Termination("Can't add non-optional argument after optional argument");
			}
			_lastAddedArgumentWasOptional = isOptional;

			_argumentNames.Add(argName);
			_argumentOptionality.Add(isOptional);
		}

		public (int max, int min) GetOptionalityRange()
		{
			int i = 0;
			while (i < _argumentOptionality.Count && _argumentOptionality[i] == false) {
				i++;
			}
			return (_argumentOptionality.Count, i);
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

			for (int i = 0; i < _argumentNames.Count; i++) {
				if (sb[sb.Length - 1] != ' ') sb.Append(' ');
				if (_argumentOptionality[i]) {
					sb.Append('[').Append(_argumentNames[i]).Append(']').Append(' ');
				}
				else {
					sb.Append('<').Append(_argumentNames[i]).Append('>').Append(' ');
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
