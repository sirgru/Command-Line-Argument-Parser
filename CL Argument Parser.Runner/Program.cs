using CLAP;
using System;

namespace CL_Argument_Parser.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
			var setup = new Setup();

			var sw1 = new CommandSwitch("n");
			sw1.AddArgument("ag1");
			sw1.AddArgument("ag2", isOptional: true);
			sw1.AddHelp("Lorem");

			var command = new Command(setup);
			command.AddSwitch(sw1);

			Console.WriteLine(command.GetHelp());
		}
    }
}
