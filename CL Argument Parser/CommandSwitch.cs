using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLAP
{
	public class CommandSwitch
	{
		private string			_primaryName;
		private HashSet<string> _alternativeNames;
		private List<string>	_paramNames = new List<string>();
		private List<bool>		_parameterOptionality = new List<bool>();
		private bool            _lastAddedArgumentWasOptional = false;
		private string			_help;

		public string primaryName => _primaryName;
		public bool acceptsArguments => _paramNames?.Count > 0;
		public int argumentsCount => _paramNames?.Count ?? 0;
		public IReadOnlyList<string> paramNames => _paramNames;
		public IReadOnlyList<string> altNames => _alternativeNames.ToList();

		public CommandSwitch(string primaryName)
		{
			_primaryName = primaryName;
		}

		public void AddParameter(string paramName, bool isOptional = false)
		{
			if (_lastAddedArgumentWasOptional && isOptional == false) {
				throw new Termination("Can't add non-optional argument after optional argument");
			}
			_lastAddedArgumentWasOptional = isOptional;

			_paramNames.Add(paramName);
			_parameterOptionality.Add(isOptional);
		}

		public (int max, int min) GetOptionalityRange()
		{
			int i = 0;
			while (i < _parameterOptionality.Count && _parameterOptionality[i] == false) {
				i++;
			}
			return (_parameterOptionality.Count, i);
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

			AddParameters(sb);

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
			if (setup.useSlash) {
				sb.Append('/').Append(name);
				return;
			}
			throw new Termination("Should never reach this.");
		}

		private void AddParameters(StringBuilder sb)
		{
			// For a single parameter display tight
			if (_paramNames.Count == 1) {
				if (_parameterOptionality[0]) {
					sb.Append('[').Append('=').Append(_paramNames[0]).Append(']').Append(' ');
				}
				else {
					sb.Append('<').Append('=').Append(_paramNames[0]).Append('>').Append(' ');
				}
				return;
			}
			else {
				for (int i = 0; i < _paramNames.Count; i++) {
					if (sb[sb.Length - 1] != ' ') sb.Append(' ');
					if (_parameterOptionality[i]) {
						sb.Append('[').Append(_paramNames[i]).Append(']').Append(' ');
					}
					else {
						sb.Append('<').Append(_paramNames[i]).Append('>').Append(' ');
					}
				}
			}
		}
	}
}
