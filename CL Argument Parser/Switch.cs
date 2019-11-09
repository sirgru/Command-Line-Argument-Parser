using System.Collections.Generic;

namespace CL_Argument_Parser
{
	public class Switch
	{
		private readonly string _name;
		private List<string> _arguments;

		public string name => _name;
		public IReadOnlyList<string> arguments => _arguments ?? new List<string>();

		public Switch(string name)
		{
			_name = name;
		}

		internal void AddArgument(string arg) {
			if (_arguments == null) _arguments = new List<string>();
			_arguments.Add(arg);
		}
	}
}