#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Examples.Data;
using Barchart.BinarySerializer.Schemas.Factories;

using Console = Barchart.BinarySerializer.Examples.Common.Console;

#endregion

namespace Barchart.BinarySerializer.Examples.Scripts;

public class SimpleEntityLookup : IScript
{
    #region Properties

    public Script Script => Script.SIMPLE_ENTITY_LOOKUP;
    
    #endregion

    #region Methods

    public void Execute()
    {
        int step = 1;

        Console.WriteStep(ref step, "Creating a dictionary to hold automobile instances");
        
        IDictionary<string, Automobile> automobiles = new Dictionary<string, Automobile>();

        Console.WriteStep(ref step, "Populating the dictionary with two automobiles");
        
        var delorian = new Automobile { Vin = "123-ABC", Make = "Delorian", Model = "DMC-12", Odometer = 0 };
        var roadster = new Automobile { Vin = "789-XYZ", Make = "Tesla", Model = "Roadster", Odometer = 93000000 };
        
        automobiles.Add(delorian.Vin, delorian);
        automobiles.Add(roadster.Vin, roadster);
        
        Console.WriteDetails(delorian.ToString(), true, false);
        Console.WriteDetails(roadster.ToString(), false, true);
        
        Console.WriteStep(ref step, "Generating a schema for the Automobile class (via reflection)");
        
        var schemaFactory = new SchemaFactory();
        var schema = schemaFactory.Make<Automobile>();

        Console.WriteStep(ref step, "Processing an update for one of the existing automobiles");
        
        var update = new byte[]
        {
            0b00000111, 0b00000000, 0b00110001, 0b00110010,
            0b00110011, 0b00101101, 0b01000001, 0b01000010,
            0b01000011, 0b11001011, 0b00000000, 0b00000000,
            0b00000000, 0b00000000
        };

        var reader = new DataBufferReader(update);
        
        Console.WriteStep(ref step, "Extracting the key value from the update");
        
        string key = schema.ReadKey<string>(reader, "Vin");

        Console.WriteStep(ref step, $"Key for the affected automobile instance is [ {key} ]");
        
        var target = automobiles[key];
        
        Console.WriteStep(ref step, $"Updating automobile [ {key} ]");
        
        schema.Deserialize(reader, target);
        
        Console.WriteStep(ref step, $"Automobile [ {key} ] state updated");
        Console.WriteDetails(target.ToString());
    }

    #endregion
}