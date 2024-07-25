#region Using Statements

using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers;

public class SerializerTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;


    #endregion

    #region Constructor(s)

    public SerializerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        
    }

    #endregion

    #region Test Methods (Serializer)
    
    
   

    #endregion

    #region Test Methods (Deserialize)

    

    #endregion

    #region Nested Types

    public class TestEntity
    {
        [Serialize(true)]
        public string KeyProperty { get; set; } = "";

        [Serialize(false)]
        public string ValueProperty { get; set; } = "";
    }

    #endregion
}