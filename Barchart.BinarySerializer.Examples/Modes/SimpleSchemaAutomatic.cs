using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Common;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas.Factories;

namespace Barchart.BinarySerializer.Examples.Modes;

public class SimpleSchemaAutomatic : IExecutable
{
    private int _step = 1;
        
    public void Execute()
    {
        Helper.WriteStep(ref _step, "Creating a SchemaFactory");
        
        var schemaFactory = new SchemaFactory();
        
        Helper.WriteStep(ref _step, "Generating a schema for the Person class (via reflection)");
        
        var schema = schemaFactory.Make<Person>();
        
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