using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Common;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;

namespace Barchart.BinarySerializer.Examples.Modes;

public class SimpleSchemaManual : IExecutable
{
    private int _step = 1;
    
    public void Execute()
    {
        Helper.WriteStep(ref _step, "Generating a schema for [Person] class manually");
        
        var schema = new Schema<Person>(new ISchemaItem<Person>[]
        {
            new SchemaItem<Person, String>("Name", false, (p) => p.Name, (p, value) => p.Name = value, new BinarySerializerString()),
            new SchemaItem<Person, ushort>("Age", false, (p) => p.Age, (p, value) => p.Age = value, new BinarySerializerUShort()),
            new SchemaItem<Person, bool>("IsProgrammer", false, (p) => p.IsProgrammer, (p, value) => p.IsProgrammer = value, new BinarySerializerBool())
        });
        
        Helper.WriteStep(ref _step, "Constructing a sample instance of the [Person] class");

        var bryan = new Person { Name = "Bryan", Age = 49, IsProgrammer = true };
        
        Helper.WriteDetails(bryan.ToString());
        
        Helper.WriteStep(ref _step, "Serializing the sample [Person] instance to binary");

        var writer = new DataBufferWriter(new byte[16]);
        var bytes = schema.Serialize(writer, bryan);
        
        Helper.WriteDetails(bytes);
        
        Helper.WriteStep(ref _step, $"Deserializing binary data into new [Person] instance");

        var reader = new DataBufferReader(bytes);

        var bryanToo = schema.Deserialize(reader);
        
        Helper.WriteDetails(bryanToo.ToString());
    }
}