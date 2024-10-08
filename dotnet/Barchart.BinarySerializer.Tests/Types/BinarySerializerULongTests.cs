#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types;

public class BinarySerializerULongTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly BinarySerializerULong _serializer;

    #endregion

    #region Constructor(s)

    public BinarySerializerULongTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        _serializer = new BinarySerializerULong();
    }

    #endregion

    #region Test Methods (Encode)

    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    [InlineData((ulong)1)]
    [InlineData((ulong)123456789)]
    public void Encode_Various_WritesExpectedBytes(ulong value)
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
            byte expectedByte = expectedBytes[i];
            byte actualByte = bytesWritten[0][i];

            Assert.Equal(expectedByte, actualByte);
        }
    }

    #endregion

    #region Test Methods (Decode)

    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    [InlineData((ulong)1)]
    [InlineData((ulong)123456789)]
    public void Decode_VariousEncoded_ReturnsExpectedValue(ulong value)
    {
        Mock<IDataBufferReader> mock = new();

        mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(value));

        ulong deserialized = _serializer.Decode(mock.Object);

        Assert.Equal(value, deserialized);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Theory]
    [InlineData(new[] { ulong.MaxValue, ulong.MaxValue })]
    [InlineData(new[] { ulong.MinValue, ulong.MinValue })]
    [InlineData(new[] { ulong.MaxValue, ulong.MinValue })]
    [InlineData(new[] { ulong.MinValue, ulong.MaxValue })]
    [InlineData(new ulong[] { 1, 123456789 })]
    public void GetEquals_Various_MatchesIEquatableOutput(ulong[] uLongs)
    {
        bool actual = _serializer.GetEquals(uLongs[0], uLongs[1]);
        bool expected = uLongs[0].Equals(uLongs[1]);

        Assert.Equal(expected, actual);
    }

    #endregion
}