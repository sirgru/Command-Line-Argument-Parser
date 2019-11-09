using CLAP;
using System;

namespace CL_Argument_Parser.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
			var setup = new Setup();

			var sw1 = new CommandSwitch("no-post-rewrite-for-whatever-reason-very-long", acceptsArguments: true);
			sw1.AddAlternativeName("n");
			sw1.AddHelp("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.");

			var sw2 = new CommandSwitch("s", acceptsArguments: true);
			sw2.AddHelp("At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga.");

			var command = new Command(setup);
			command.AddSwitch(sw1);
			command.AddSwitch(sw2);

			Console.WriteLine(command.GetHelp());
		}
    }
}
