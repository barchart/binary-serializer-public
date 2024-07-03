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

        mock.Setup(m => m.ReadBytes(2)).Returns(BitConverter.GetBytes(value));

        var deserialized = _serializer.Decode(mock.Object);

        Assert.Equal(value, deserialized);
    }

    #endregion

    #region Test Methods (GetEquals)
    [Theory]
    [InlineData(new[] {'a', 'a'})]
    [InlineData(new[] {'b', 'b'})]
    [InlineData(new[] {'A', 'A'})]
    [InlineData(new[] {'a', 'b'})]
    [InlineData(new[] {'A', 'B'})]
    public void GetEquals_Various_MatchesIEquatableOutput(char[] chars)
    {
        var actual = _serializer.GetEquals(chars[0], chars[1]);
        var expected = chars[0].Equals(chars[1]);
        
        Assert.Equal(expected, actual);
    }

    #endregion
}