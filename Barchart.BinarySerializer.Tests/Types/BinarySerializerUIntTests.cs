#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerUIntTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;
        
        private readonly BinarySerializerUInt _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerUIntTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _serializer = new BinarySerializerUInt();
        }

        #endregion

        #region Test Methods (Encode)

        [Theory]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData((uint)1)]
        [InlineData((uint)123456789)]
        public void Encode_Various_WritesExpectedBytes(uint value)
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
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData((uint)1)]
        [InlineData((uint)123456789)]
        public void Decode_VariousEncoded_ReturnsExpectedValue(uint value)
        {
            Mock<IDataBufferReader> mock = new();

            mock.Setup(m => m.ReadBytes(4)).Returns(BitConverter.GetBytes(value));

            var deserialized = _serializer.Decode(mock.Object);

            Assert.Equal(value, deserialized);
        }

        #endregion

        #region Test Methods (GetEquals)

        [Theory]
        [InlineData(new uint[] { uint.MaxValue, uint.MaxValue })]
        [InlineData(new uint[] { uint.MinValue, uint.MinValue })]
        [InlineData(new uint[] { uint.MaxValue, uint.MinValue })]
        [InlineData(new uint[] { uint.MinValue, uint.MaxValue })]
        [InlineData(new uint[] { 1, 123456789 })]
        public void GetEquals_Various_MatchesIEquatableOutput(uint[] uints)
        {
            var actual = _serializer.GetEquals(uints[0], uints[1]);
            var expected = uints[0].Equals(uints[1]);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}