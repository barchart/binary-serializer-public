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
        Mock<IDataBufferReader> mock = new Mock<IDataBufferReader>();

        mock.Setup(m => m.ReadBit()).Returns(true);

        bool deserialized = _serializer.Decode(mock.Object);
        
        Assert.True(deserialized);
    }
        
    [Fact]
    public void Decode_SerializedFalseValue_ReturnsFalse()
    {
        Mock<IDataBufferReader> mock = new Mock<IDataBufferReader>();

        mock.Setup(m => m.ReadBit()).Returns(false);

        bool deserialized = _serializer.Decode(mock.Object);
        
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
}