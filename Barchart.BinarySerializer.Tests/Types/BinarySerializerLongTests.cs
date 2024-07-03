#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerLongTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;
        
        private readonly BinarySerializerLong _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerLongTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            _serializer = new BinarySerializerLong();
        }

        #endregion

        #region Test Methods (Encode)

        [Theory]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(1L)]
        public void Encode_Various_WritesExpectedBytes(long value)
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
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(1L)]
        public void Decode_VariousEncoded_ReturnsExpectedValue(long value)
        {
            Mock<IDataBufferReader> mock = new();

            mock.Setup(m => m.ReadBytes(8)).Returns(BitConverter.GetBytes(value));

            var deserialized = _serializer.Decode(mock.Object);

            Assert.Equal(value, deserialized);
        }

        #endregion

        #region Test Methods (GetEquals)

        [Theory]
        [InlineData(new long[] { long.MaxValue, long.MaxValue })]
        [InlineData(new long[] { long.MinValue, long.MinValue })]
        [InlineData(new long[] { 0L, 0L })]
        [InlineData(new long[] { long.MaxValue, long.MinValue })]
        [InlineData(new long[] { long.MinValue, long.MaxValue })]
        [InlineData(new long[] { 1L, -1L })]
        public void GetEquals_Various_MatchesIEquatableOutput(long[] longs)
        {
            var actual = _serializer.GetEquals(longs[0], longs[1]);
            var expected = longs[0].Equals(longs[1]);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}