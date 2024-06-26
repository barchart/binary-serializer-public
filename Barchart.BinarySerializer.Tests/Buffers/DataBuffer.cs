#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Tests.Buffers
{
    public class DataBufferTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;

        #endregion

        #region Constructor(s)

        public DataBufferTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region Test Methods (ReadByte)

        [Fact]
        public void ReadByte_Once_ReturnsFirstByte()
        {
            byte first;

            var byteArray = new[] { first = 250 };
            var dataBuffer = new DataBufferReader(byteArray);

            byte readFirst = dataBuffer.ReadByte();

            Assert.Equal(first, readFirst);
        }

        [Fact]
        public void ReadByte_Twice_ReturnsSecondByte()
        {
            byte first;
            byte second;

            var byteArray = new[] { first = 250, second = 175 };
            var dataBuffer = new DataBufferReader(byteArray);

            byte readFirst = dataBuffer.ReadByte();
            byte readSecond = dataBuffer.ReadByte();

            Assert.Equal(first, readFirst);
            Assert.Equal(second, readSecond);
        }

        #endregion

        #region Test Methods (WriteBit)

        [Fact]
        public void WriteBit_True_ModifiesBuffer()
        {
            var byteArray = new byte[1];
            var dataBuffer = new DataBufferWriter(byteArray);

            dataBuffer.WriteBit(true);

            Assert.True(GetBit(byteArray[0], 0));
        }

        [Fact]
        public void WriteBit_False_ModifiesBuffer()
        {
            var byteArray = new byte[1];
            var dataBuffer = new DataBufferWriter(byteArray);

            dataBuffer.WriteBit(false);

            Assert.False(GetBit(byteArray[0], 0));
        }

        [Theory]
        [InlineData(new[] { true, true })]
        [InlineData(new[] { false, false })]
        [InlineData(new[] { true, true, false, true })]
        [InlineData(new[] { false, false, true, false })]
        [InlineData(new[] { true, false, true, false, true, false, true, false })]
        [InlineData(new[] { false, true, false, true, false, true, false, true })]
        public void WriteBit_Multiple_ModifiesBuffer(bool[] bits)
        {
            var byteArray = new byte[1];
            var dataBuffer = new DataBufferWriter(byteArray);

            for (int i = 0; i < bits.Length; i++)
            {
                dataBuffer.WriteBit(bits[i]);
            }

            _testOutputHelper.WriteLine(PrintBits(byteArray[0]));

            for (int i = 0; i < bits.Length; i++)
            {
                Assert.Equal(GetBit(byteArray[0], i), bits[i]);
            }
        }

        #endregion

        #region Test Methods (ToBytes)

        [Fact]
        public void ToBytes_WhenZeroBitsAreWritten_ReturnsEmptyArray()
        {
            var byteArray = new byte[2];
            var dataBuffer = new DataBufferWriter(byteArray);

            var bytes = dataBuffer.ToBytes();

            Assert.Empty(bytes);
        }

        [Fact]
        public void ToBytes_WhenOneBitIsWritten_ReturnsOneByteArray()
        {
            var byteArray = new byte[2];
            var dataBuffer = new DataBufferWriter(byteArray);

            for (int i = 0; i < 1; i++)
            {
                dataBuffer.WriteBit(i % 2 == 0);
            }

            var bytes = dataBuffer.ToBytes();

            Assert.Single(bytes);
        }

        [Fact]
        public void ToBytes_WhenOneByteIsWritten_ReturnsOneByteArray()
        {
            var byteArray = new byte[2];
            var dataBuffer = new DataBufferWriter(byteArray);

            for (int i = 0; i < 8; i++)
            {
                dataBuffer.WriteBit(i % 2 == 0);
            }

            var bytes = dataBuffer.ToBytes();

            Assert.Single(bytes);
        }

        [Fact]
        public void ToBytes_WhenTwelveBitsAreWritten_ReturnsTwoBytesArray()
        {
            var byteArray = new byte[2];
            var dataBuffer = new DataBufferWriter(byteArray);

            for (int i = 0; i < 12; i++)
            {
                dataBuffer.WriteBit(i % 2 == 0);
            }

            var bytes = dataBuffer.ToBytes();

            Assert.Equal(2, bytes.Length);
        }

        [Fact]
        public void ToBytes_WhenTwoBytesAreWritten_ReturnsTwoBytesArray()
        {
            var byteArray = new byte[2];
            var dataBuffer = new DataBufferWriter(byteArray);

            for (int i = 0; i < 16; i++)
            {
                dataBuffer.WriteBit(i % 2 == 0);
            }

            var bytes = dataBuffer.ToBytes();

            Assert.Equal(2, bytes.Length);
        }

        #endregion

        #region Static Methods

        private static bool GetBit(byte b, int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Bit index cannot be less than zero.");
            }

            if (index > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Bit index cannot be greater than seven.");
            }

            return ((b >> (7 - index)) & 1) == 1;
        }

        private static string PrintBits(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        #endregion
    }
}