using CLAP;
using System;
using System.Text;

namespace CL_Argument_Parser.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
			var swA = new CommandSwitch(primaryName: "AAA", shortName: 'a');
			swA.AddParameter(arity: Arity.One, name: "a-param");
			swA.SetHelp("Help for A.");

			var swB = new CommandSwitch(primaryName: "be");
			swB.AddParameter(arity: Arity.NoneOrOne, name: "b-param");
			swB.SetHelp("Help for B.");

			var parser = new Parser();
			parser.AddSwitch(swA);
			parser.AddSwitch(swB);

			StringBuilder sb = new StringBuilder();
			parser.GetHelp(setup: new Setup(), builder: sb);
			Console.WriteLine(sb.ToString());
		}
    }
}
