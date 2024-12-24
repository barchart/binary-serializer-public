#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;

using Console = Barchart.BinarySerializer.Examples.Common.Console;

#endregion

namespace Barchart.BinarySerializer.Examples.Scripts;

/// <summary>
///     Demonstrates the process of looking up and updating an entity in a dictionary using the binary serializer.
/// </summary>
public class SimpleEntityLookup : IScript
{
    #region Constants

    private const int AUTOMOBILE_ENTITY_ID = 1;
    
    #endregion
    
    #region Properties

    /// <inheritdoc />
    public Script Script => Script.SIMPLE_ENTITY_LOOKUP;
    
    #endregion

    #region Methods

    /// <inheritdoc />
    public void Execute()
    {
        int step = 1;
        
        Console.WriteStep(ref step, "Creating a dictionary to hold automobile instances");
        
        IDictionary<string, Automobile> automobiles = new Dictionary<string, Automobile>();

        Console.WriteStep(ref step, "Populating the dictionary with two automobiles");
        
        Automobile delorian = new() { Vin = "123-ABC", Make = "Delorian", Model = "DMC-12", Odometer = 0 };
        Automobile roadster = new() { Vin = "789-XYZ", Make = "Tesla", Model = "Roadster", Odometer = 93000000 };
        
        automobiles.Add(delorian.Vin, delorian);
        automobiles.Add(roadster.Vin, roadster);
        
        Console.WriteDetails(delorian.ToString(), true, false);
        Console.WriteDetails(roadster.ToString(), false);
        
        Console.WriteStep(ref step, "Generating a schema for the Automobile class (via reflection)");
        
        SchemaFactory schemaFactory = new();
        ISchema<Automobile> schema = schemaFactory.Make<Automobile>(AUTOMOBILE_ENTITY_ID);
        
        Console.WriteStep(ref step, "Processing an update for one of the existing automobiles");
        
        var update = new byte[]
        {
            0b00000001, 0b00000011, 0b10000000, 0b00011000,
            0b10011001, 0b00011001, 0b10010110, 0b10100000,
            0b10100001, 0b00100001, 0b11100101, 0b10000000,
            0b00000000, 0b00000000, 0b00000000
        };

        DataBufferReader reader = new(update);
        
        Console.WriteStep(ref step, "Extracting the key value from the update");

        string key = schema.ReadKey<string>(reader, "Vin");

        Console.WriteStep(ref step, $"Key for the affected automobile instance is [ {key} ]");
        
        Automobile target = automobiles[key];
        
        Console.WriteStep(ref step, $"Updating automobile [ {key} ]");
        
        schema.Deserialize(reader, target);
        
        Console.WriteStep(ref step, $"Automobile [ {key} ] state updated");
        Console.WriteDetails(target.ToString());
    }

    #endregion
}