using Barchart.BinarySerializer.Attributes;

namespace Barchart.BinarySerializer.Examples.Data;

public class Automobile
{
    [Serialize(true)]
    public string Vin { get; set; }
    
    [Serialize]
    public string Make { get; set; }
    
    [Serialize]
    public string Model { get; set; }
    
    [Serialize]
    public uint Odometer { get; set; }

    public Automobile()
    {
        Vin = "";

        Make = "";
        Model = "";

        Odometer = 0;
    }
    
    public override string ToString()
    {
        return $"(Automobile [Vin={Vin}, Make={Make}, Model={Model}, Odometer={Odometer}])";
    }
}