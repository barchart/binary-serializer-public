#region Using Statements

using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Types.Factories;

public class BinaryTypeSerializerFactoryTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinaryTypeSerializerFactory _factory;
    
    #endregion
    
    #region Constructor(s)
        
    public BinaryTypeSerializerFactoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _factory = new BinaryTypeSerializerFactory();
    }
    
    #endregion
    
    #region Test Methods (Encode)
        
    [Fact]
    public void Make_TestEnum_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<TestEnum>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<TestEnum>>(serializer);
    }
    
    #endregion
    
    #region Nested Types
        
    private enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
        
    #endregion
}