using System.Collections.Generic;

namespace CL_Argument_Parser
{
	public class Switch
	{
		public string name;
		private List<string> _arguments;

		public IReadOnlyList<string> arguments => _arguments;
		public int argumentsCount => _arguments == null ? 0 : _arguments.Count;

		public Switch(string name)
		{
			this.name = name;
		}

		internal void AddArgument(string arg) {
			if (_arguments == null) _arguments = new List<string>();
			_arguments.Add(arg);
		}

		public override bool Equals(object obj)
		{
			var o = obj as Switch;
			if (o == null) return false;
			return name == o.name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
	}
}