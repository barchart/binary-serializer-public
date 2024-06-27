#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerBoolTests
    {
        #region Fields
    
        private readonly ITestOutputHelper _testOutputHelper;
        
        private readonly BinarySerializerBool _serializer;
        
        #endregion
        
        #region Constructor(s)
        
        public BinarySerializerBoolTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _serializer = new BinarySerializerBool();
        }
    
        #endregion

        #region Test Methods (Encode)
        
        [Fact]
        public void Encode_True_WritesToDataBuffer()
        {
            var mock = new Mock<IDataBufferWriter>();

            var bitsWritten = new List<bool>();
            var bytesWritten = new List<byte[]>();
            
            mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
            mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
            _serializer.Encode(mock.Object, true);
            
            Assert.False(bitsWritten[0]);
            Assert.False(bitsWritten[1]);
            Assert.True(bitsWritten[2]);
            Assert.Empty(bytesWritten);
        }
        
        [Fact]
        public void Encode_False_WritesToDataBuffer()
        {
            var mock = new Mock<IDataBufferWriter>();

            var bitsWritten = new List<bool>();
            var bytesWritten = new List<byte[]>();
            
            mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
            mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
            _serializer.Encode(mock.Object, false);
            
            Assert.False(bitsWritten[0]);
            Assert.False(bitsWritten[1]);
            Assert.False(bitsWritten[2]);
            Assert.Empty(bytesWritten);
        }

        #endregion

        #region Test Methods (Decode)

        [Fact]
        public void Decode_DataBufferWithSerializedtrueValue_ReturnsHeaderWithDecodedValue()
        {
            var byteArray = BitsToBytes(new bool[] { false, false, true }) ;
            IDataBufferReader dataBuffer = new DataBufferReader(byteArray);
            
            Attribute<bool> attribute = _serializer.Decode(dataBuffer);
            
            Assert.False(attribute.IsValueMissing);
            Assert.True(attribute.Value);
        }
        
        [Fact]
        public void Decode_DataBufferWithSerializedFalseValue_ReturnsHeaderWithDecodedValue()
        {
            var byteArray = BitsToBytes(new bool[] { false, false, false }) ;
            IDataBufferReader dataBuffer = new DataBufferReader(byteArray);

            Attribute<bool> attribute = _serializer.Decode(dataBuffer);

            Assert.False(attribute.IsValueMissing);
            Assert.False(attribute.Value);
        }

        private static byte[] BitsToBytes(bool[] bits)
        {
            int byteCount = (int)Math.Ceiling(bits.Length / 8.0); 
            byte[] bytes = new byte[byteCount];

            int byteIndex = 0;
            int bitIndex = 7;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    bytes[byteIndex] |= (byte)(1 << bitIndex);
                }

                bitIndex--;
                if (bitIndex < 0)
                {
                    byteIndex++;
                    bitIndex = 7;
                }
            }

            return bytes;
        }

        #endregion
    }
}