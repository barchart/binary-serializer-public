using Barchart.BinarySerializer.Examples;
using Barchart.BinarySerializer.Examples.Modes;

using Console = Barchart.BinarySerializer.Examples.Common.Console;

Console.WriteLine("Barchart.BinarySerializer.Examples Console Application");
Console.WriteLine();

Script type = default(Script);

if (args.Length != 0)
{
    Enum.TryParse(args[0], out type);
}

IScript script;

if (type == Script.SIMPLE_SCHEMA_AUTOMATIC)
{
    script = new SimpleSchemaAutomatic();
} 
else if (type == Script.SIMPLE_SCHEMA_MANUAL)
{
    script = new SimpleSchemaManual();
}
else
{
    script = new SimpleSchemaAutomatic();
}

Console.WriteLine($"Starting [{script.Script}] script");
Console.WriteLine();

script.Execute();

Console.WriteLine($"Finished [{script.Script}] script");
