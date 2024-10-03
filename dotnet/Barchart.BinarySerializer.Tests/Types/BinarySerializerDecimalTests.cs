#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Tests.Common;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerDecimalTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly BinarySerializerDecimal _serializer;
    
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerDecimalTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerDecimal();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    [InlineData("0.0000000000000000000000000001")]
    [InlineData("3.14159265359")]
    public void Encode_Various_WritesExpectedBytes(string valueString)
    {
        decimal value = decimal.Parse(valueString);
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
        
        int[] components = decimal.GetBits(value);
        byte[] expectedBytes = new byte[16];
        
        for (int i = 0; i < 4; i++)
        {
            BitConverter.GetBytes(components[i]).CopyTo(expectedBytes, i * 4);
        }
        
        Assert.Equal(4, bytesWritten.Count);

        byte[] bytes = Helpers.CombineFourByteArrays(bytesWritten);
        Assert.Equal(expectedBytes.Length, bytes.Length);
        
        for (int i = 0; i < expectedBytes.Length; i++)
        {
            byte expectedByte = expectedBytes[i];
            byte actualByte = bytes[i];
            
            Assert.Equal(expectedByte, actualByte);
        }
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    [InlineData("0.0000000000000000000000000001")]
    [InlineData("3.14159265359")]
    public void Decode_VariousEncoded_ReturnsExpectedValue(string expectedString)
    {
        decimal expectedValue = decimal.Parse(expectedString);
        int[] components = decimal.GetBits(expectedValue);

        Mock<IDataBufferReader> mock = new();
        mock.SetupSequence(m => m.ReadBytes(4))
            .Returns(BitConverter.GetBytes(components[0]))
            .Returns(BitConverter.GetBytes(components[1]))
            .Returns(BitConverter.GetBytes(components[2]))
            .Returns(BitConverter.GetBytes(components[3]));

        decimal actualValue = _serializer.Decode(mock.Object);

        Assert.Equal(expectedValue, actualValue);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData("0", "0")]
    [InlineData("1", "1")]
    [InlineData("-1", "-1")]
    [InlineData("79228162514264337593543950335", "79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335", "-79228162514264337593543950335")]
    [InlineData("0.0000000000000000000000000001", "0.0000000000000000000000000001")]
    [InlineData("3.14159265359", "3.14159265359")]
    [InlineData("1", "-1")]
    [InlineData("0.1", "0.2")]
    public void GetEquals_Various_ReturnsExpectedResult(string first, string second)
    {
        decimal a = decimal.Parse(first);
        decimal b = decimal.Parse(second);

        bool actual = _serializer.GetEquals(a, b);
        bool expected = first.Equals(second);

        Assert.Equal(expected, actual);
    }
    
    #endregion
}