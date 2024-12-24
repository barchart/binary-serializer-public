#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Headers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Tests.Headers;

public class BinaryHeaderSerializerTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinaryHeaderSerializer _serializer;
    
    #endregion
    
    #region Constructor(s)
        
    public BinaryHeaderSerializerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = BinaryHeaderSerializer.Instance;
    }
    
    #endregion
    
    #region Test Methods (Encode)
    
    [Fact]
    public void Encode_ZeroFalse_ReturnsCorrectByte()
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
        _serializer.Encode(mock.Object, 0, false);
            
        Assert.Empty(bitsWritten);
        Assert.Single(byteWritten);
        Assert.Empty(bytesWritten);

        byte serialized = byteWritten[0];
        
        Assert.Equal(0b00000000, serialized);
    }
    
    [Fact]
    public void Encode_FifteenTrue_ReturnsCorrectByte()
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
        _serializer.Encode(mock.Object, 15, true);
            
        Assert.Empty(bitsWritten);
        Assert.Single(byteWritten);
        Assert.Empty(bytesWritten);

        byte serialized = byteWritten[0];
        
        Assert.Equal(0b10001111, serialized);
    }
    
    [Fact]
    public void Encode_SixteenTrue_ThrowsInvalidHeaderException()
    {
        Mock<IDataBufferWriter> mock = new();

        List<bool> bitsWritten = new();
        List<byte> byteWritten = new();
        List<byte[]> bytesWritten = new();
            
        mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
        mock.Setup(m => m.WriteByte(Capture.In(byteWritten)));
        mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));

        Assert.Throws<InvalidHeaderException>(() => _serializer.Encode(mock.Object, 16, true));
    }
    
    #endregion
    
    #region Test Methods (Decode)
    
    [Fact]
    public void Decode_ZeroFalse_ReturnsExpectedHeader()
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns(0b00000000);
        
        Header header = _serializer.Decode(mock.Object);
        
        Assert.Equal(0, header.EntityId);
        Assert.False(header.Snapshot);
    }
    
    [Fact]
    public void Decode_FifteenTrue_ReturnsExpectedHeader()
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns(0b10001111);
        
        Header header = _serializer.Decode(mock.Object);
        
        Assert.Equal(15, header.EntityId);
        Assert.True(header.Snapshot);
    }
    
    [Fact]
    public void Decode_SixteenTrue_Throws()
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns(0b10010000);
        
        Assert.Throws<InvalidHeaderException>(() => _serializer.Decode(mock.Object));
    }
    
    #endregion
}