# CLAP: Command Line Argument Parser

This project is a simple implementation of an argument parser for command line applications.

Key features:
* Ability to use '`-`' for single-char switches.
* Ability to use '`--`' or '`/`' for multi-char switches.
* Ability to set up which switch prefixes are used.
* Ability to repeat switches with correct result.
* Switches can take arguments (zero, one or more).
* Switches can have synonyms.


Allowed formats:
1. `<paths> <switches>`
2. `<switches> <paths>`
3. `<paths> <switches> <paths> <switches> ...`
4. `<switches> <paths> <switches> <paths> ...`

Paths are command arguments separate from the switches. They can be any string which doesn't start with a switch marker, not necessarily a path (although we refer to them as "paths"). 

If the last switch before paths takes arguments, `<paths>` will be considered to be the arguments of that switch. Otherwise, paths will be aggregated as arguments of the command. This can be counteracted by using switch format #2 shown below.

Switch format:
1. `<marker><name> [switch-arguments]`  
where marker is one of ('`-`', '`--`' or '`/`'); and switch-arguments are arguments that do not start with a switch marker, separated by spaces.
2. `<marker><name>[=switch-argument]`, without spacing.

It returns a data structure containing:
* A list of paths.
* A list of switch data items, where each item consists of:
  * Data about the switch itself, not related to user input:
    * Primary name (e.g. `--switch`)
    * Alternative names (e.g. `-s`)
    * Other data
  * Data about the switch arguments:
    * Parameter name
    * Argument value


## Example:

```C#
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
sw1.AddParameter("sw-arg", isOptional: true);
// Add help for that switch
sw1.AddHelp("Help here.");

// Create a command, which will transform the parse results to the meaningful format
var command = new Command(setup);
command.AddSwitch(sw1);
// result is what we can use
var result = command.Adapt(parserResult);
			
Assert.AreEqual(1, result.paths.Count);
Assert.AreEqual("p1", result.paths[0]);
Assert.AreEqual(1, result.switches.Count);
Assert.AreEqual("switch", result.switches[0].commandSwitch.primaryName);
Assert.AreEqual("s", result.switches[0].commandSwitch.altNames[0]);
Assert.AreEqual("param", result.switches[0].arguments[0].paramName);
Assert.AreEqual("switch-arg", result.switches[0].arguments[0].argName);
```

Calling `command.GetHelp()` gives:

```
All available switches:

    --switch, -s[=param]  Help here.
```



