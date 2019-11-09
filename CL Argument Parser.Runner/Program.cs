using CLAP;
using System;

namespace CL_Argument_Parser.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
			// Showing the defaults
			var setup = new Setup(useDash: true, useDoubleDash: true, useSlash: false);
			// Create a parser
			var parser = new Parser(setup);
			// Parse the input. The process is not complete, parserResult contains only
			// results without semantic meaning 
			var parserResult = parser.Parse(new string[] {"p1", "-s", "switch-arg", "--switch",  });

			// Create a switch
			var sw1 = new CommandSwitch("switch");
			// Add alternative name for the switch
			sw1.AddAlternativeName("s");
			// Add an optional parameter for the switch
			sw1.AddParameter("param", isOptional: true);
			// Add help for that switch
			sw1.AddHelp("Help here.");

			// Create a command, which will transform the parse results to the meaningful format
			var command = new Command(setup);
			command.AddSwitch(sw1);
			// result is what we can use
			var result = command.Adapt(parserResult);

			Console.WriteLine(command.GetHelp());
		}
    }
}
