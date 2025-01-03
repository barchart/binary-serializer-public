#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerEnumTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinarySerializerEnum<TestEnum> _serializer;

    #endregion
    
    #region Constructor(s)
    
    public BinarySerializerEnumTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerEnum<TestEnum>(new BinarySerializerInt());
    }

    #endregion

    #region Test Methods (Encode)
    
    [Theory]
    [InlineData(TestEnum.ValueOne)]
    [InlineData(TestEnum.ValueOneMillion)]
    [InlineData(TestEnum.ValueOneBillion)]
    public void Encode_Various_WritesExpectedBytes(TestEnum value)
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
        Assert.Empty(byteWritten);
    
        int intValue = Convert.ToInt32(value);
        byte[] expectedBytes = BitConverter.GetBytes(intValue);
    
        Assert.Single(bytesWritten);
        Assert.Equal(expectedBytes.Length, bytesWritten[0].Length);
    
        for (int i = 0; i < expectedBytes.Length; i++)
        {
            byte expectedByte = expectedBytes[i];
            byte actualByte = bytesWritten[0][i];
        
            Assert.Equal(expectedByte, actualByte);
        }
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(TestEnum.ValueOne)]
    [InlineData(TestEnum.ValueOneMillion)]
    [InlineData(TestEnum.ValueOneBillion)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(TestEnum expectedValue)
    {
        int intValue = Convert.ToInt32(expectedValue);

        Mock<IDataBufferReader> mock = new();
        mock.Setup(m => m.ReadBytes(4)).Returns(BitConverter.GetBytes(intValue));

        TestEnum actualValue = _serializer.Decode(mock.Object);

        Assert.Equal(expectedValue, actualValue);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [InlineData(new[] { TestEnum.ValueOne, TestEnum.ValueOne })]
    [InlineData(new[] { TestEnum.ValueOneMillion, TestEnum.ValueOneMillion })]
    [InlineData(new[] { TestEnum.ValueOneBillion, TestEnum.ValueOneBillion })]
    [InlineData(new[] { TestEnum.ValueOne, TestEnum.ValueOneMillion })]
    [InlineData(new[] { TestEnum.ValueOneMillion, TestEnum.ValueOneBillion })]
    [InlineData(new[] { TestEnum.ValueOne, TestEnum.ValueOneBillion })]
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
        ValueOne = 1,
        ValueOneMillion = 1000000,
        ValueOneBillion = 1000000000
    }
    
    #endregion
}