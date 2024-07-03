#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerByteTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerByte _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerByteTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerByte();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MinValue)]
    [InlineData((byte)127)]
    [InlineData((byte)128)]
    public void Encode_Various_WritesExpectedBytes(byte value)
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
        Assert.Equal(value, byteWritten[0]);
        
        Assert.Empty(bytesWritten);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MinValue)]
    [InlineData((byte)127)]
    [InlineData((byte)128)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(byte value)
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns(value);

        var deserialized = _serializer.Decode(mock.Object);
        
        Assert.Equal(value, deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { byte.MaxValue, byte.MaxValue })]
    [InlineData(new[] { byte.MinValue, byte.MinValue })]
    [InlineData(new[] { (byte)128, (byte)128 })]
    [InlineData(new[] { byte.MaxValue, byte.MinValue })]
    [InlineData(new[] { byte.MinValue, byte.MaxValue })]
    [InlineData(new[] { (byte)128, (byte)127 })]
    public void GetEquals_Various_MatchesIEquatableOutput(byte[] bytes)
    {
        var actual = _serializer.GetEquals(bytes[0], bytes[1]);
        var expected = bytes[0].Equals(bytes[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}