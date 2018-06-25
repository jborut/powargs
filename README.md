# PowArg
_Powershell like arguments for your .Net project_

Aim of this library is to easily parse array of strings (arguments) and convert them to a custom defined POCO object.

## Usage

Simply define a POCO class that will define your arguments as properties. Add argument attribute to each property you wish to be parsed:

```csharp
public class Arguments
{
    [PowArg.Argument.Argument("Int argument")]
    public int IntArg { get; set; } = 10;

    [PowArg.Argument.Argument("Float argument")]
    public float FloatArg { get; set; } = 11f;

    [PowArg.Argument.Argument("Decimal argument")]
    public decimal DecimalArg { get; set; } = 12M;

    [PowArg.Argument.Argument("String argument")]
    public string StringArg { get; set; } = "string'";

    [PowArg.Argument.Argument("Boolean argument")]
    public bool BoolArg { get; set; } = false;
}
```

Argument attribute requires description of the property which will be used to generate help text. It can also accept *required* switch which defaults to *false*.

```csharp
[PowArg.Argument.Argument("Argument description", required: false)]
```

If argument is not passed to _Parser_ the default value defined in the class will be used, unless the argument is marked as required in which case _Parser_ will throw an exception.

_Parser_ can parse arguments based on position or name.

### Position based arguments

```csharp
private static void Main(string[] args)
{
    // args = { "2", "3", "4", "5", "true" }

    var arguments = PowArg.Parser<Arguments>.Parse(args);

    // arguments.IntArg == 2;
    // arguments.FloatArg == 3f;
    // arguments.DecimalArg == 4M;
    // arguments.StringArg == "5";
    // arguments.BoolArg == true;
}
```

### Name based arguments

```csharp
private static void Main(string[] args)
{
    /* 
    args = 
    { 
        "-BoolArg", "true", 
        "-StringArg", "2",
        "-DecimalArg", "3",
        "-FloatArg", "4",
        "-IntArg", "5" 
    }
    */

    var arguments = PowArg.Parser<Arguments>.Parse(args);

    // arguments.IntArg == 5;
    // arguments.FloatArg == 4f;
    // arguments.DecimalArg == 3M;
    // arguments.StringArg == "2";
    // arguments.BoolArg == true;
}
```

Boolean arguments don't require value. If value is not defined, they will be set to *true*:

```csharp
var arguments = PowArg.Parser<Arguments>.Parse(new[]
{
    "-IntArg", "5",
    "-BoolArg"
});

// arguments.BoolArg == true;
```

### Argument name

By default argument names will start with **-** character (i.e. -Name). This can be changed by passing the string prefix to Parser method:

```csharp
// use / as argument name prefix
PowArg.Parser.Parse(args, "/");
```

### Argument position

When parsing arguments based on position in array, the correct position in POCO class is determined by the line they appear in source file, in ascending order. In other words, arguments in array will be matched with properties how they appear in ascending order in class where they are defined.

### Exceptions

_Parser_ will throw exception in this cases:

- if there are missing required arguments
- if some properties could not be parsed

## Help

Library can generate help text that describes all the arguments.

```csharp
string[] help = Helper<Arguments>.GetHelpText();

Console.WriteLine(string.Join("\n", help));
```

Will produce output like this:

```
-IntArg <int> (10)                      Int argument
[-FloatArg <float> (11)]                Float argument
[-DecimalArg <decimal> (12)]            Decimal argument
[-StringArg <string> (string)]          String argument
[-BoolArg <bool> (False)]               Boolean argument

```

In this case -IntArg is required argument, others are optional. Value in __()__ is a default value (if defined).

## More Examples

Feel free to take a look at the [Unit tests](https://github.com/jborut/powarg/blob/master/UnitTests/TestParser.cs).