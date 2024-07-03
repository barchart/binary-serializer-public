#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerUShortTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;
        
        private readonly BinarySerializerUShort _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerUShortTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            _serializer = new BinarySerializerUShort();
        }

        #endregion

        #region Test Methods (Encode)

        [Theory]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData((ushort)1)]
        [InlineData((ushort)12345)]
        public void Encode_Various_WritesExpectedBytes(ushort value)
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
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData((ushort)1)]
        [InlineData((ushort)12345)]
        public void Decode_VariousEncoded_ReturnsExpectedValue(ushort value)
        {
            Mock<IDataBufferReader> mock = new();

            mock.Setup(m => m.ReadBytes(2)).Returns(BitConverter.GetBytes(value));

            var deserialized = _serializer.Decode(mock.Object);

            Assert.Equal(value, deserialized);
        }

        #endregion

        #region Test Methods (GetEquals)

        [Theory]
        [InlineData(new ushort[] { ushort.MaxValue, ushort.MaxValue })]
        [InlineData(new ushort[] { ushort.MinValue, ushort.MinValue })]
        [InlineData(new ushort[] { ushort.MaxValue, ushort.MinValue })]
        [InlineData(new ushort[] { ushort.MinValue, ushort.MaxValue })]
        [InlineData(new ushort[] { 1, 12345 })]
        public void GetEquals_Various_MatchesIEquatableOutput(ushort[] ushorts)
        {
            var actual = _serializer.GetEquals(ushorts[0], ushorts[1]);
            var expected = ushorts[0].Equals(ushorts[1]);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}