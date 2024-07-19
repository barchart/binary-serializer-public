#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerSByteTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinarySerializerSByte _serializer;

    #endregion

    #region Constructor(s)

    public BinarySerializerSByteTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        _serializer = new BinarySerializerSByte();
    }

    #endregion

    #region Test Methods (Encode)

    [Theory]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    [InlineData((sbyte)0)]
    [InlineData((sbyte)-1)]
    [InlineData((sbyte)1)]
    public void Encode_Various_WritesExpectedBytes(sbyte value)
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
        Assert.Empty(bytesWritten);

        Assert.Single(byteWritten);
        Assert.Equal((byte)value, byteWritten[0]);
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    [InlineData((sbyte)0)]
    [InlineData((sbyte)-1)]
    [InlineData((sbyte)1)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(sbyte value)
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadByte()).Returns((byte)value);

        var deserialized = _serializer.Decode(mock.Object);

        Assert.Equal(value, deserialized);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [InlineData(new[] { sbyte.MaxValue, sbyte.MaxValue })]
    [InlineData(new[] { sbyte.MinValue, sbyte.MinValue })]
    [InlineData(new[] { (sbyte)0, (sbyte)0 })]
    [InlineData(new[] { sbyte.MaxValue, sbyte.MinValue })]
    [InlineData(new[] { sbyte.MinValue, sbyte.MaxValue })]
    [InlineData(new[] { (sbyte)1, (sbyte)-1 })]
    public void GetEquals_Various_MatchesIEquatableOutput(sbyte[] sbytes)
    {
        var actual = _serializer.GetEquals(sbytes[0], sbytes[1]);
        var expected = sbytes[0].Equals(sbytes[1]);

        Assert.Equal(expected, actual);
    }

    #endregion
}
