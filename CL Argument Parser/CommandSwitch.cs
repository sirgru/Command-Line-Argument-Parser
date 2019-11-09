using System.Collections.Generic;

namespace CL_Argument_Parser
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
	}
}
