#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerEnumByteTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinarySerializerEnumByte<TestEnum> _serializer;

    #endregion
    
    #region Constructor(s)
    
    public BinarySerializerEnumByteTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerEnumByte<TestEnum>(new BinarySerializerByte());
    }

    #endregion

    #region Test Methods (Encode)
    
    [Theory]
    [InlineData(TestEnum.Value1)]
    [InlineData(TestEnum.Value2)]
    [InlineData(TestEnum.Value3)]
    [InlineData(TestEnum.Value255)]
    public void Encode_NormalValues_WritesExpectedBytes(TestEnum value)
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
        
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
    
        _serializer.Encode(mock.Object, value);
        
        Assert.Empty(bitsWritten);
        Assert.Single(byteWritten);
        Assert.Empty(bytesWritten);
        
        byte expectedByte = Convert.ToByte(value);
        
        Assert.Equal(expectedByte, byteWritten[0]);
    }
    
    [Theory]
    [InlineData(TestEnum.Value256)]
    [InlineData(TestEnum.Value1000)]
    public void Encode_LargeValues_Throws(TestEnum value)
    {
        Mock<IDataBufferWriter> mock = new();

        Assert.Throws<OverflowException>(() => _serializer.Encode(mock.Object, value));
    }
    
    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(TestEnum.Value1)]
    [InlineData(TestEnum.Value2)]
    [InlineData(TestEnum.Value3)]
    [InlineData(TestEnum.Value255)]
    public void Decode_NormalEncodedValues_ReturnsExpectedValue(TestEnum expectedValue)
    {
        byte byteValue = Convert.ToByte(expectedValue);

        Mock<IDataBufferReader> mock = new();
        mock.Setup(m => m.ReadByte()).Returns(byteValue);

        TestEnum actualValue = _serializer.Decode(mock.Object);

        Assert.Equal(expectedValue, actualValue);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [InlineData(new[] { TestEnum.Value1, TestEnum.Value1 })]
    [InlineData(new[] { TestEnum.Value2, TestEnum.Value2 })]
    [InlineData(new[] { TestEnum.Value3, TestEnum.Value3 })]
    [InlineData(new[] { TestEnum.Value1, TestEnum.Value2 })]
    [InlineData(new[] { TestEnum.Value2, TestEnum.Value3 })]
    [InlineData(new[] { TestEnum.Value1, TestEnum.Value3 })]
    public void GetEquals_Various_ReturnsExpectedResult(TestEnum[] testEnums)
    {
        bool actual = _serializer.GetEquals(testEnums[0], testEnums[1]);
        bool expected = testEnums[0].Equals(testEnums[1]);

        Assert.Equal(expected, actual);
    }

    #endregion
    
    #region Nested Types
    
    public enum TestEnum
    {
        Value1 = 1,
        Value2 = 2,
        Value3 = 3,
        Value255 = 255,
        Value256 = 256,
        Value1000 = 1000
    }
    
    #endregion
}