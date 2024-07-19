#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas.Factories;
using Console = Barchart.BinarySerializer.Examples.Common.Console;

#endregion

namespace Barchart.BinarySerializer.Examples.Scripts;

public class SimpleSchemaAutomatic : IScript
{
    #region Properties
    public Script Script => Script.SIMPLE_SCHEMA_AUTOMATIC;

    #endregion

    #region Methods

    public void Execute()
    {
        int step = 1;
    
        Console.WriteStep(ref step, "Creating a SchemaFactory");
        
        var schemaFactory = new SchemaFactory();
        
        Console.WriteStep(ref step, "Generating a schema for the Person class (via reflection)");
        
        var schema = schemaFactory.Make<Person>();
        
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