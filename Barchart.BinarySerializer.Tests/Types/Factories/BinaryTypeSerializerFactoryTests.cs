#region Using Statements

using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Types.Exceptions;
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
    
    #region Test Methods (Make)
        
    [Fact]
    public void Make_Boolean_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<bool>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<bool>>(serializer);
    }
    
    [Fact]
    public void Make_Byte_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<byte>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<byte>>(serializer);
    }
    
    [Fact]
    public void Make_Char_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<char>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<char>>(serializer);
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
        var serializer = _factory.Make<decimal>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<decimal>>(serializer);
    }
    
    [Fact]
    public void Make_Double_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<double>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<double>>(serializer);
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
        var serializer = _factory.Make<float>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<float>>(serializer);
    }
    
    [Fact]
    public void Make_Int32_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<int>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<int>>(serializer);
    }
    
    [Fact]
    public void Make_Int64_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<long>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<long>>(serializer);
    }
    
    [Fact]
    public void Make_SByte_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<sbyte>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<sbyte>>(serializer);
    }
    
    [Fact]
    public void Make_Int16_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<short>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<short>>(serializer);
    }
    
    [Fact]
    public void Make_String_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<string>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<string>>(serializer);
    }
    
    [Fact]
    public void Make_UInt32_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<uint>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<uint>>(serializer);
    }
    
    [Fact]
    public void Make_UInt64_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<ulong>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<ulong>>(serializer);
    }
    
    [Fact]
    public void Make_UInt16_ReturnsTypedSerializer()
    {
        var serializer = _factory.Make<ushort>();

        Assert.IsAssignableFrom<IBinaryTypeSerializer<ushort>>(serializer);
    }
    
    [Fact]
    public void Make_TestClass_ThrowsUnsupportedTypeException()
    {
        Assert.Throws<UnsupportedTypeException>(() => _factory.Make<TestClass>());
    }
    
    #endregion
    
    #region Nested Types
        
    private enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }

    private class TestClass
    {
        
    }
        
    #endregion
}