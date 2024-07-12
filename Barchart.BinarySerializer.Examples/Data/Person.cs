using Barchart.BinarySerializer.Attributes;

namespace Barchart.BinarySerializer.Examples.Data;

public class Person
{
    [Serialize]
    public String Name { get; set; }
    
    [Serialize]
    public bool IsProgrammer { get; set; }
    
    [Serialize]
    public ushort Age { get; set; }
    
    public Person()
    {
        Name = "";
        IsProgrammer = false;
        Age = 0;
    }

    public override string ToString()
    {
        return $"(Person [Name={Name}, IsProgrammer={IsProgrammer}, Age={Age}])";
    }
}