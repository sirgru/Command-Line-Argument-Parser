using System.Collections.Generic;

namespace CLAP
{
	public class ParseResult
	{
		internal List<string> paths = new List<string>();
		internal List<Switch> switches = new List<Switch>();

		public IReadOnlyList<string> Paths => paths;
		public IReadOnlyList<Switch> Switches => switches;

		internal Switch TryAddSwitch(string name)
		{
			var existingSw = switches.Find(x => x.name == name);
			if (existingSw == null) {
				var sw = new Switch(name);
				switches.Add(sw);
				return sw;
			}
			else {
				return existingSw;
			}
		}
	}
}
