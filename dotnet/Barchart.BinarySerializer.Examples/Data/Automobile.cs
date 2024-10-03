#region Using Statements

using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Examples.Data;

public class Automobile
{
    #region Properties

    [Serialize(true)]
    public string Vin { get; set; }
    
    [Serialize]
    public string Make { get; set; }
    
    [Serialize]
    public string Model { get; set; }
    
    [Serialize]
    public uint Odometer { get; set; }

    #endregion

    #region Constructor(s)
    public Automobile()
    {
        Vin = "";

        Make = "";
        Model = "";

        Odometer = 0;
    }

    #endregion
    
    #region Methods

    public override string ToString()
    {
        return $"(Automobile [Vin={Vin}, Make={Make}, Model={Model}, Odometer={Odometer}])";
    }
    
    #endregion
}