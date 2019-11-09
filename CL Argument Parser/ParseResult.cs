using System.Collections.Generic;

namespace CLAP
{
	public class ParseResult
	{
		internal List<string> paths = new List<string>();
		internal List<TextSwitch> switches = new List<TextSwitch>();

		public IReadOnlyList<string> Paths => paths;
		public IReadOnlyList<TextSwitch> Switches => switches;

		internal TextSwitch TryAddSwitch(string name)
		{
			if (name.Contains('=')) {
				var split = name.Split('=');
				name = split[0];
				var sw = Add(name);
				sw.AddTightArgument(split[1]);
				return sw;
			}
			else return Add(name);
		}

		private TextSwitch Add(string name)
		{
			var existingSw = switches.Find(x => x.name == name);
			if (existingSw == null) {
				var sw = new TextSwitch(name);
				switches.Add(sw);
				return sw;
			}
			else {
				return existingSw;
			}
		}

		internal TextSwitch TryAddManySwitches(string names)
		{
			if (names[0] == '=') throw new InputException("Invalid switch argument format.");

			var lastSwitch = Add(names[0].ToString());

			for (int i = 1; i < names.Length; i++) {
				if (names[i] == '=') {
					if (i == names.Length - 1) throw new InputException("Invalid switch argument format.");
					lastSwitch.AddTightArgument(names.Substring(i + 1));
					break;
				} else {
					lastSwitch = Add(names[i].ToString());
				}
			}
			return lastSwitch;
		}
	}
}
