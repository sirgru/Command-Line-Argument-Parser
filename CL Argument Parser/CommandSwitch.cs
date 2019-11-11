using System;
using System.Text;

namespace CLAP
{
	public enum Arity
	{
		None, One, NoneOrOne, Any
	}

	public class CommandSwitch
	{
		private string	 _longName;
		private char	 _shortName;
		private string[] _alternativeNames;
		private string	 _help;
		private Arity	 _arity;
		private string	 _parameterName;
		private bool	 _isImportant;

		internal Arity arity => _arity;

		public string primaryName => (_longName != null) ? _longName : _shortName.ToString();

		/// <summary>
		/// A command may be created by a primary name, which may have 1 or more characters. 
		/// If it has only 1 character, it will be invoked with '-c' switch, where 'c' is that character.
		/// If it has more characters, it will be invoked with '--name'. 
		/// In addition to the primary name, a command may have a short name, which is always 1 character,
		/// and serves as an alias to the main command.
		/// Optional parameter isImportant is ued when displaying help, where 
		/// only switches marked important will be shown.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public CommandSwitch(string primaryName, char shortName = '\0', bool isImportant = true)
		{
			if (primaryName == null || primaryName.Length == 0) throw new ArgumentException("Invalid primary name");
			if (primaryName.Length == 1 && shortName != '\0') throw new ArgumentException("Primary name length cannot be 1, when the short name is present.");

			if (primaryName.Length == 1) {
				_longName = null;
				_shortName = primaryName[0];
			} else {
				_longName = primaryName;
				_shortName = shortName;
			}

			_isImportant = isImportant;
			_arity = Arity.None;
		}

		/// <summary>
		/// Adds a parameter for the switch. 
		/// When switches are parsed, NoneOrOne type of switches 
		/// must have the argument present in format '=value',
		/// following the switch without spaces.
		/// Help shows to users correctly how to do this.
		/// One and Many types of switches get their arguments
		/// separated by spaces.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public void AddParameter(Arity arity, string name)
		{
			if (arity == Arity.None) throw new ArgumentException("Can't explicitly set switch arity to none.");
			_arity = arity;
			_parameterName = name;
		}

		/// <summary>
		/// Besides the primary name, switches may have additional names. 
		/// They are used internally for matching and displayed in help as alternatives to main switches.
		/// </summary>
		/// <param name="altNames"></param>
		public void AddAlternativeNames(params string[] altNames)
		{
			foreach (var altName in altNames) {
				if (altName == null || altName.Length == 0) throw new ArgumentException("Alt names cannot be empty or null.");
			}
			_alternativeNames = altNames;
		}

		public void SetHelp(string help)
		{
			_help = help;
		}

		internal bool IsIdentifiedBy(string other)
		{
			return _longName == other || (other.Length == 1 && other[0] == _shortName) || _alternativeNames.Contains(other); 
		}

		#region Help
		internal void GetHelp(Setup setup, StringBuilder sb, bool onlyImportant = false)
		{
			if (onlyImportant && !_isImportant) return;

			// Start counting names length
			int startingLength = sb.Length;

			sb.Append(' ', setup.leftMargin);

			AppendShortName(sb);

			AppendPrimaryName(sb);

			AppendAltNames(sb);

			AppendParameters(sb);

			// When names are wider than namesWidth, 
			// Overflow help text to a new line, from that width
			int namesWidthNoWS = (sb[sb.Length - 1] == ' ') ? sb.Length - startingLength - 1 : sb.Length - startingLength;
			int namesWidth = sb.Length - startingLength;
			if (namesWidthNoWS >= setup.namesWidth) {
				sb.Append('\n');
				sb.Append(' ', setup.namesWidth);
			} else {
				sb.Append(' ', setup.namesWidth - namesWidth);
			}

			var (help, hasMoreLines) = TextFormatter.Format(_help, setup.lineWidth, setup.namesWidth, 0, setup.lineWidth - setup.namesWidth, false);
			sb.Append(help).Append('\n');
			if (hasMoreLines) sb.Append('\n');
		}

		private void AppendShortName(StringBuilder sb)
		{
			if (_shortName != '\0') sb.Append('-').Append(_shortName).Append(' ');
			else sb.Append(' ', 3);
		}

		private void AppendPrimaryName(StringBuilder sb)
		{
			if (_longName != null) sb.Append("--").Append(_longName);
		}

		private void AppendAltNames(StringBuilder sb)
		{
			if (_alternativeNames != null) {
				bool firstPass = true;
				foreach (var altName in _alternativeNames) {
					if (firstPass) {
						if (_longName != null) sb.Append(", ");
						firstPass = false;
					}
					sb.Append("--").Append(altName);
				}
			}
		}

		private void AppendParameters(StringBuilder sb)
		{
			switch (_arity) {
			case Arity.None: return;

			case Arity.One:
				// Make sure there is space between name and params
				if (sb[sb.Length - 1] != ' ') sb.Append(' ');
				sb.Append('<').Append(_parameterName).Append('>').Append(' ');
				return;

			case Arity.NoneOrOne:
				// No space here, even backtrack if no long names
				if (_longName == null && _alternativeNames == null) sb.Length -= 1;

				sb.Append('[').Append('=').Append(_parameterName).Append(']').Append(' ');
				return;

			case Arity.Any:
				// Make sure there is space between name and params
				if (sb[sb.Length - 1] != ' ') sb.Append(' ');
				sb.Append('<').Append(_parameterName).Append("...").Append('>').Append(' ');
				return;

			default: throw new Termination("Should never reach this.");
			}
		}
		#endregion Help
	}
}
