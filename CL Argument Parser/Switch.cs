using System.Collections.Generic;

namespace CLAP
{
	public class TextSwitch
	{
		public string name;
		private List<string> _arguments;
		private List<string> _tightArguments;

		public IReadOnlyList<string> arguments => _arguments;
		public IReadOnlyList<string> tightArguments => _tightArguments;
		public int argumentsCount => _arguments?.Count ?? 0;
		public int tightArgumentsCount => _tightArguments?.Count ?? 0;

		public TextSwitch(string name)
		{
			this.name = name;
		}

		internal void AddArgument(string arg) {
			if (_arguments == null) _arguments = new List<string>();
			_arguments.Add(arg);
		}

		internal void AddTightArgument(string arg)
		{
			if (_tightArguments == null) _tightArguments = new List<string>();
			_tightArguments.Add(arg);
		}

		public override bool Equals(object obj)
		{
			var o = obj as TextSwitch;
			if (o == null) return false;
			return name == o.name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
	}
}