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
    public void Make_Boolean_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Boolean>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Boolean>>(serializer);
    }
    
    [Fact]
    public void Make_Byte_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Byte>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Byte>>(serializer);
    }
    
    [Fact]
    public void Make_Char_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Char>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Char>>(serializer);
    }
    
    [Fact]
    public void Make_DateOnly_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<DateOnly>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<DateOnly>>(serializer);
    }
    
    [Fact]
    public void Make_DateTime_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<DateTime>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<DateTime>>(serializer);
    }
    
    [Fact]
    public void Make_Decimal_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Decimal>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Decimal>>(serializer);
    }
    
    [Fact]
    public void Make_Double_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Double>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Double>>(serializer);
    }
    
    [Fact]
    public void Make_TestEnum_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<TestEnum>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<TestEnum>>(serializer);
    }
    
    [Fact]
    public void Make_Single_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Single>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Single>>(serializer);
    }
    
    [Fact]
    public void Make_Int32_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Int32>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Int32>>(serializer);
    }
    
    [Fact]
    public void Make_Int64_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Int64>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Int64>>(serializer);
    }
    
    [Fact]
    public void Make_SByte_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<SByte>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<SByte>>(serializer);
    }
    
    [Fact]
    public void Make_Int16_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<Int16>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<Int16>>(serializer);
    }
    
    [Fact]
    public void Make_String_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<String>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<String>>(serializer);
    }
    
    [Fact]
    public void Make_UInt32_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<UInt32>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<UInt32>>(serializer);
    }
    
    [Fact]
    public void Make_UInt64_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<UInt64>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<UInt64>>(serializer);
    }
    
    [Fact]
    public void Make_UInt16_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<UInt16>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<UInt16>>(serializer);
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