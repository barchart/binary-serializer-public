#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;

using Console = Barchart.BinarySerializer.Examples.Common.Console;

#endregion

namespace Barchart.BinarySerializer.Examples.Scripts;

/// <summary>
///     Demonstrates automatic schema generation and (de)serialization of a `Person` instance.
/// </summary>
public class SimpleSchemaAutomatic : IScript
{
    #region Properties

    /// <inheritdoc />
    public Script Script => Script.SIMPLE_SCHEMA_AUTOMATIC;

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Execute()
    {
        int step = 1;
    
        Console.WriteStep(ref step, "Creating a SchemaFactory");
        
        SchemaFactory schemaFactory = new();
        
        Console.WriteStep(ref step, "Generating a schema for the Person class (via reflection)");
        
        ISchema<Person> schema = schemaFactory.Make<Person>();
        
        Console.WriteStep(ref step, "Constructing a sample instance of the [Person] class");

        Person bryan = new() 
        { 
            Name = "Bryan",
            Age = 49, 
            IsProgrammer = true 
        };
        
        Console.WriteDetails(bryan.ToString());
        
        Console.WriteStep(ref step, "Serializing the sample [Person] instance to binary");

        DataBufferWriter writer = new(new byte[16]);
        byte[] bytes = schema.Serialize(writer, bryan);
        
        Console.WriteDetails(bytes);
        
        Console.WriteStep(ref step, $"Deserializing binary data into new [Person] instance");

        DataBufferReader reader = new(bytes);

        Person bryanToo = schema.Deserialize(reader);
        
        Console.WriteDetails(bryanToo.ToString());
    }

    #endregion
}