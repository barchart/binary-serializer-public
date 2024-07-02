#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerBoolTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerBool _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerBoolTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerBool();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Fact]
    public void Encode_True_WritesToDataBuffer()
    {
        var mock = new Mock<IDataBufferWriter>();

        var bitsWritten = new List<bool>();
        var bytesWritten = new List<byte[]>();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
        _serializer.Encode(mock.Object, true);
            
        Assert.Single(bitsWritten);
        Assert.True(bitsWritten[0]);
            
        Assert.Empty(bytesWritten);
    }
        
    [Fact]
    public void Encode_False_WritesToDataBuffer()
    {
        var mock = new Mock<IDataBufferWriter>();

        var bitsWritten = new List<bool>();
        var bytesWritten = new List<byte[]>();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
        _serializer.Encode(mock.Object, false);
            
        Assert.Single(bitsWritten);
        Assert.False(bitsWritten[0]);
            
        Assert.Empty(bytesWritten);
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_SerializedTrueValue_ReturnsTrue()
    {
        byte[] byteArray = BitsToBytes(new[] { true });
        IDataBufferReader dataBuffer = new DataBufferReader(byteArray);
            
        bool deserialized = _serializer.Decode(dataBuffer);
            
        Assert.True(deserialized);
    }
        
    [Fact]
    public void Decode_SerializedFalseValue_ReturnsFalse()
    {
        byte[] byteArray = BitsToBytes(new[] { false });
        IDataBufferReader dataBuffer = new DataBufferReader(byteArray);
            
        bool deserialized = _serializer.Decode(dataBuffer);
            
        Assert.False(deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { true, true })]
    [InlineData(new[] { false, false })]
    [InlineData(new[] { false, true })]
    [InlineData(new[] { true, false })]
    public void GetEquals_Multiple_MatchesIEquatableResult(bool[] bits)
    {
        bool actual = _serializer.GetEquals(bits[0], bits[1]);
        bool expected = bits[0].Equals(bits[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
        
    #region Static Methods
        
    private static byte[] BitsToBytes(bool[] bits)
    {
        int byteCount = (int)Math.Ceiling(bits.Length / 8.0); 
        byte[] bytes = new byte[byteCount];

        int byteIndex = 0;
        int bitIndex = 7;

        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i])
            {
                bytes[byteIndex] |= (byte)(1 << bitIndex);
            }

            bitIndex--;
            if (bitIndex < 0)
            {
                byteIndex++;
                bitIndex = 7;
            }
        }

        return bytes;
    }
        
    #endregion
}