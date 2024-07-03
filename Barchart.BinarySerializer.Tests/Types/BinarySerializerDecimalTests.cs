#region Using Statements

using Barchart.BinarySerializer.Buffers;
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

        _serializer.Encode(mock.Object, value);
            
        int[] bits = decimal.GetBits(value);
        byte[] expectedBytes = new byte[16];
        Buffer.BlockCopy(bits, 0, expectedBytes, 0, 16);
        
        mock.Verify(m => m.WriteBytes(It.Is<byte[]>(b => b.SequenceEqual(expectedBytes))), Times.Once);
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
        decimal expected = decimal.Parse(expectedString);
        Mock<IDataBufferReader> mock = new();
        
        int[] bits = decimal.GetBits(expected);
        byte[] bytes = new byte[16];
        Buffer.BlockCopy(bits, 0, bytes, 0, 16);
        
        mock.Setup(m => m.ReadBytes(16)).Returns(bytes);

        var actual = _serializer.Decode(mock.Object);
        
        Assert.Equal(expected, actual);
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
    public void GetEquals_Various_MatchesIEquatableOutput(string a, string b)
    {
        decimal first = decimal.Parse(a);
        decimal second = decimal.Parse(b);
        var actual = _serializer.GetEquals(first, second);
        var expected = a.Equals(b);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}