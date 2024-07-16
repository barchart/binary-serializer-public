using Barchart.BinarySerializer.Examples.Scripts;

using Console = Barchart.BinarySerializer.Examples.Common.Console;

Console.WriteLine("Barchart.BinarySerializer.Examples Console Application");
Console.WriteLine();

Script type = default(Script);

if (args.Length != 0)
{
    Enum.TryParse(args[0], out type);
}

Console.WriteLine(type.ToString());

IScript script;

if (type == Script.SIMPLE_SCHEMA_AUTOMATIC)
{
    script = new SimpleSchemaAutomatic();
} 
else if (type == Script.SIMPLE_SCHEMA_MANUAL)
{
    script = new SimpleSchemaManual();
}
else if (type == Script.SIMPLE_ENTITY_LOOKUP)
{
    script = new SimpleEntityLookup();
}
else
{
    script = new SimpleSchemaAutomatic();
}

Console.WriteLine($"Starting [{script.Script}] script");
Console.WriteLine();

script.Execute();

Console.WriteLine($"Finished [{script.Script}] script");