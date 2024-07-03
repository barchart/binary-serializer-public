#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerShortTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;
        
        private readonly BinarySerializerShort _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerShortTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            _serializer = new BinarySerializerShort();
        }

        #endregion

        #region Test Methods (Encode)

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        public void Encode_Various_WritesExpectedBytes(short value)
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
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        public void Decode_VariousEncoded_ReturnsExpectedValue(short value)
        {
            Mock<IDataBufferReader> mock = new();

            mock.Setup(m => m.ReadBytes(2)).Returns(BitConverter.GetBytes(value));

            var deserialized = _serializer.Decode(mock.Object);

            Assert.Equal(value, deserialized);
        }

        #endregion

        #region Test Methods (GetEquals)

        [Theory]
        [InlineData(new short[] { short.MaxValue, short.MaxValue })]
        [InlineData(new short[] { short.MinValue, short.MinValue })]
        [InlineData(new short[] { 0, 0 })]
        [InlineData(new short[] { short.MaxValue, short.MinValue })]
        [InlineData(new short[] { short.MinValue, short.MaxValue })]
        [InlineData(new short[] { 1, -1 })]
        public void GetEquals_Various_MatchesIEquatableOutput(short[] shorts)
        {
            var actual = _serializer.GetEquals(shorts[0], shorts[1]);
            var expected = shorts[0].Equals(shorts[1]);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}