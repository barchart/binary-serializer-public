#region Using Statements

using System.Text;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerStringTests
    {
        #region Fields

        private readonly BinarySerializerString _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerStringTests()
        {
            _serializer = new BinarySerializerString();
        }

        #endregion

        #region Test Methods (Encode)

      

        #endregion

        #region Test Methods (Decode)

        [Theory]
        [InlineData("Testing Encoding & Decoding methods")]
        [InlineData("")]
        [InlineData("Binary Serialization")]
        public void Decode_VariousEncodedInUTF8_ReturnsExpectedValue(string value)
        {
            Mock<IDataBufferReader> mock = new();

            byte[] encodedBytes = new byte[2];
            mock.Setup(m => m.ReadBytes(2)).Returns(BitConverter.GetBytes(value.Length));
            mock.Setup(m => m.ReadBytes(value.Length)).Returns(Encoding.UTF8.GetBytes(value));

            var deserialized = _serializer.Decode(mock.Object);

            Assert.Equal(value, deserialized);
        }

        #endregion

        #region Test Methods (GetEquals)

        [Theory]
        [InlineData(new object[] { new string[] { "Test", "Test" }})]
        [InlineData(new object[] { new string[] { "String", "string" }})]
        [InlineData(new object[] { new string[] { "", "" }})]
        [InlineData(new object[] { new string[] { "Binary", "Serialization" }})]
        public void GetEquals_Various_MatchesIEquatableOutput(string[] strings)
        {
            var actual = _serializer.GetEquals(strings[0], strings[1]);
            var expected = strings[0].Equals(strings[1]);
            
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}