#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerIntTests
{
    #region Fields
    
    private readonly ITestOutputHelper _testOutputHelper;
        
    private readonly BinarySerializerInt _serializer;
        
    #endregion
        
    #region Constructor(s)
        
    public BinarySerializerIntTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _serializer = new BinarySerializerInt();
    }
    
    #endregion

    #region Test Methods (Encode)
        
    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    public void Encode_Various_WritesExpectedBytes(int value)
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
        Assert.Empty(byteWritten);
        
        byte[] expectedBytes = BitConverter.GetBytes(value);
        
        Assert.Single(bytesWritten);
        Assert.Equal(expectedBytes.Length, bytesWritten[0].Length);
        
        for (int i = 0; i < expectedBytes.Length; i++)
        {
            var expectedByte = expectedBytes[i];
            var actualByte = bytesWritten[0][i];
            
            Assert.Equal(expectedByte, actualByte);
        }
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(int value)
    {
        Mock<IDataBufferReader> mock = new Mock<IDataBufferReader>();
        
        mock.Setup(m => m.ReadBytes(4)).Returns(BitConverter.GetBytes(value));

        var deserialized = _serializer.Decode(mock.Object);
        
        Assert.Equal(value, deserialized);
    }

    #endregion
    
    #region Test Methods (GetEquals)
    
    [Theory]
    [InlineData(new[] { int.MaxValue, int.MaxValue })]
    [InlineData(new[] { int.MinValue, int.MinValue })]
    [InlineData(new[] { 0, 0 })]
    [InlineData(new[] { int.MaxValue, int.MinValue })]
    [InlineData(new[] { int.MinValue, int.MaxValue })]
    [InlineData(new[] { 1, -1 })]
    public void GetEquals_Various_MatchesIEquatableOutput(int[] integers)
    {
        var actual = _serializer.GetEquals(integers[0], integers[1]);
        var expected = integers[0].Equals(integers[1]);
        
        Assert.Equal(expected, actual);
    }
    
    #endregion
}