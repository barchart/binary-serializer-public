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
    [InlineData(Byte.MaxValue)]
    [InlineData(Byte.MinValue)]
    [InlineData((byte)127)]
    [InlineData((byte)128)]
    public void Encode_Various_WritesExpectedBytes(byte value)
    {
        var mock = new Mock<IDataBufferWriter>();

        var bitsWritten = new List<bool>();
        var byteWritten = new List<byte>();
        var bytesWritten = new List<byte[]>();
            
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
    [InlineData(Byte.MaxValue)]
    [InlineData(Byte.MinValue)]
    [InlineData((byte)127)]
    [InlineData((byte)128)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(byte value)
    {
        Mock<IDataBufferReader> mock = new Mock<IDataBufferReader>();

        mock.Setup(m => m.ReadByte()).Returns(value);

        byte deserialized = _serializer.Decode(mock.Object);
        
        Assert.Equal(value, deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { Byte.MaxValue, Byte.MaxValue })]
    [InlineData(new[] { Byte.MinValue, Byte.MinValue })]
    [InlineData(new[] { (byte)128, (byte)128 })]
    [InlineData(new[] { Byte.MaxValue, Byte.MinValue })]
    [InlineData(new[] { Byte.MinValue, Byte.MaxValue })]
    [InlineData(new[] { (byte)128, (byte)127 })]
    public void GetEquals_Various_MatchesIEquatableOutput(byte[] bytes)
    {
        bool actual = _serializer.GetEquals(bytes[0], bytes[1]);
        bool expected = bytes[0].Equals(bytes[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}