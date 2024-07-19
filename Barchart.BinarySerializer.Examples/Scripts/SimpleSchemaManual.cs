#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;
using Console = Barchart.BinarySerializer.Examples.Common.Console;

#endregion

namespace Barchart.BinarySerializer.Examples.Scripts;

public class SimpleSchemaManual : IScript
{
    #region Properties

    public Script Script => Script.SIMPLE_SCHEMA_MANUAL;

    #endregion
    
    #region Methods

    public void Execute()
    {
        int step = 1;
        
        Console.WriteStep(ref step, "Generating a schema for [Person] class manually");
        
        var schema = new Schema<Person>(new ISchemaItem<Person>[]
        {
            new SchemaItem<Person, string>("Name", false, (p) => p.Name, (p, value) => p.Name = value, new BinarySerializerString()),
            new SchemaItem<Person, ushort>("Age", false, (p) => p.Age, (p, value) => p.Age = value, new BinarySerializerUShort()),
            new SchemaItem<Person, bool>("IsProgrammer", false, (p) => p.IsProgrammer, (p, value) => p.IsProgrammer = value, new BinarySerializerBool())
        });
        
        Console.WriteStep(ref step, "Constructing a sample instance of the [Person] class");

        var bryan = new Person { Name = "Bryan", Age = 49, IsProgrammer = true };
        
        Console.WriteDetails(bryan.ToString());
        
        Console.WriteStep(ref step, "Serializing the sample [Person] instance to binary");

        var writer = new DataBufferWriter(new byte[16]);
        var bytes = schema.Serialize(writer, bryan);
        
        Console.WriteDetails(bytes);
        
        Console.WriteStep(ref step, $"Deserializing binary data into new [Person] instance");

        var reader = new DataBufferReader(bytes);

        var bryanToo = schema.Deserialize(reader);
        
        Console.WriteDetails(bryanToo.ToString());
    }

    #endregion
}