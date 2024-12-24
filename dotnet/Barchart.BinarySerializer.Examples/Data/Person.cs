#region Using Statements

using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Examples.Data;

public class Person
{
    #region Properties

    [Serialize]
    public string? Name { get; set; }
    
    [Serialize]
    public bool IsProgrammer { get; set; }
    
    [Serialize]
    public ushort Age { get; set; }
    
    #endregion
    
    #region Constructor(s)

    public Person()
    {
        Name = "";
        IsProgrammer = false;
        Age = 0;
    }

    #endregion
    
    #region Methods

    public override string ToString()
    {
        return $"(Person [Name={Name}, IsProgrammer={IsProgrammer}, Age={Age}])";
    }
    
    #endregion
}