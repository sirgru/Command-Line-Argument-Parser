# CLAP: Command Line Argument Parser

This project is a simple implementation of an argument parser for command line applications.

It takes arguments in format:

	[switches] [--] [paths]

Where:
* `switches` are arguments starting with either `-` or `--`. When starting with `-`, every following character in that argument is assumed to represent one command switch (combining of one-char switches is allowed). When prefixed with `--` the rest of the argument is assumed to be one switch. 
* `--` by itself is an optional separator between switch arguments and paths.
* `paths` are arguments which are not switches or separator. They can represent any value, not necessarily a path (although we refer to them as "paths").

Switches have the following data associated on creation:
* Primary name: a string which identifies a switch, will be matched to supplied argument on command line.
* Short name: when primary name is longer than 1 character, additional short name can be supplied on creation, of only 1 character, which is a synonym for the longer primary name. When primary name is already 1 character long this shorthand is not allowed.
* Alternative names: additional names can be associated.
* Help: help text that will be displayed when requested.
* Argument: Switches can take arguments, of the following arities:
  * None: The default, no argument.
  * One: A single argument is mandatory. The argument is the next argument passed on command line, which must not be a switch.
  * NoneOrOne: argument is part of the switch text, denoted with `=value`, not separated by spaces.
  * Any: any number of arguments, which are separate arguments on command line, counted until the next switch or separator is found.
* Importance flag: allows for displaying help items selectively.

Switches passed on command line can be repeated, with correct results in parsing with respect to arguments, long/short/alt names etc. This includes throwing exceptions in illegal situations.

After the first separator or path is encountered, all other arguments are presumed to be paths. This allows us to have arguments which are themselves starting with `-`, `--` or `--` itself.

Parsing returns a data structure containing:
* A nullable list of paths.
* A nullable list of switch data items, where each item consists of:
  * Data about the switch itself, set on creation.
  * A nullable list of arguments passed in.

The library also contains a text formatter class, which takes an input string and breaks it up so into lines of requested width, with option to set left margin and to justify.


## Example:

```C#
var swA = new CommandSwitch(primaryName: "AAA", shortName: 'a');
swA.AddParameter(arity: Arity.One, name: "a-param");
swA.SetHelp("Help for A: a very long help line which will demonstrate our line breaking system.");

var swB = new CommandSwitch(primaryName: "be");
swB.AddParameter(arity: Arity.NoneOrOne, name: "b-param");
swB.SetHelp("Help for B: a very long help line which will demonstrate our line breaking system.");

var parser = new Parser();
parser.AddSwitch(swA);
parser.AddSwitch(swB);

var result = parser.Parse(new string[] {"-a", "111", "--be=222", "path1"});

Assert.AreEqual(1, result.pathsCount);
Assert.AreEqual("path1", result.paths[0]);
Assert.AreEqual(2, result.switchesCount);
Assert.AreEqual("AAA", result.switches[0].primaryName);
Assert.AreEqual("111", result.switches[0].args[0]);
Assert.AreEqual("be", result.switches[1].primaryName);
Assert.AreEqual("222", result.switches[1].args[0]);
```

To generate help, use:

```C#
StringBuilder sb = new StringBuilder();
parser.GetHelp(setup: new Setup(), builder: sb);
Console.WriteLine(sb.ToString());
```
which gives:

```
 -a --AAA <a-param>       Help for A: a very long help line which will
                          demonstrate our line breaking system.

    --be[=b-param]        Help for B: a very long help line which will
                          demonstrate our line breaking system.
```

Using the `TextFormatter` like this:

```C#
var text = "Lorem ipsum dolor sit amet, consecteturoriumuselratietoneritumusaden adipiscing elit. Aenean nec convallis magna, in pretium tellus.\n\nPellentesque mattis arcu sed neque pretium tincidunt. Nam magna neque, convallis quis dapibus sit amet, pulvinar sit amet arcu.";
var actual = TextFormatter.Format(input: text, lineWidth: 40,   
			 leftMargin: 2, firstLineLeftMargin: 4, justify: false);
```
Gives:
```
    Lorem ipsum dolor sit amet, 
  consecteturoriumuselratietoneritumusad
  en adipiscing elit. Aenean nec 
  convallis magna, in pretium tellus.
  
  Pellentesque mattis arcu sed neque 
  pretium tincidunt. Nam magna neque, 
  convallis quis dapibus sit amet, 
  pulvinar sit amet arcu.
```
and with justify:
```C#
var actual = TextFormatter.Format(input: text, lineWidth: 40,  
             leftMargin: 2, firstLineLeftMargin: 4, justify: true);
```
Gives:
```
    Lorem    ipsum    dolor   sit  amet,
  consecteturoriumuselratietoneritumusad
  en    adipiscing    elit.  Aenean  nec
  convallis magna, in pretium tellus.
  
  Pellentesque  mattis  arcu  sed  neque
  pretium  tincidunt.  Nam  magna neque,
  convallis    quis  dapibus  sit  amet,
  pulvinar sit amet arcu.
```

`TextFormatter` also has options for formatting headers:

```C#
var actual = TextFormatter.FormatHeader("HEADER", 40, TextFormatter.OutlineType.Line, leftMargin: 10);
```
```
          ---------------- HEADER ----------------
```
```C#
var actual = TextFormatter.FormatHeader("HEADER", 40, TextFormatter.OutlineType.Equals, leftMargin: 10);
```
```
          ================ HEADER ================
```
```C#
var actual = TextFormatter.FormatHeader("HEADER", 12, TextFormatter.OutlineType.Underline, leftMargin: 5);
```
```
        HEADER   
        ------
```
