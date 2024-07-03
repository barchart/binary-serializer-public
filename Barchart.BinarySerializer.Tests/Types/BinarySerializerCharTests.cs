#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerCharTests
{
    #region Fields
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly BinarySerializerChar _serializer;
    #endregion

    #region Constructor(s)
    public BinarySerializerCharTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        _serializer = new BinarySerializerChar();
    }
    
    #endregion

    #region Test Methods (Encode)

    [Theory]
    [InlineData('a')]
    [InlineData('z')]
    [InlineData('A')]
    [InlineData('Z')]
    [InlineData('0')]
    [InlineData('9')]
    [InlineData('\u00A0')]
    public void Encode_Various_WritesExpectedBytes(char value)
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
        Assert.Equal((byte)value, byteWritten[0]);
        Assert.Empty(bytesWritten);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData('a')]
    [InlineData('z')]
    [InlineData('A')]
    [InlineData('Z')]
    [InlineData('0')]
    [InlineData('9')]
    [InlineData('\u00A0')]
    public void Decode_VariousEncoded_ReturnsExpectedValue(char value)
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns((byte)value);

        var deserialized = _serializer.Decode(mock.Object);

        Assert.Equal(value, deserialized);
    }

    #endregion

    #region Test Methods (GetEquals)
    [Theory]
    [InlineData('a', 'a')]
    [InlineData('b', 'b')]
    [InlineData('A', 'A')]
    [InlineData('a', 'b')]
    [InlineData('A', 'B')]
    public void GetEquals_Various_MatchesIEquatableOutput(char value1, char value2)
    {
        var actual = _serializer.GetEquals(value1, value2);
        var expected = value1.Equals(value2);
        Assert.Equal(expected, actual);
    }

    #endregion
}