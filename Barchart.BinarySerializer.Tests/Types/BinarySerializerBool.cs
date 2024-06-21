#region Using Statements

using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Schemas;
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
            var mock = new Mock<DataBuffer>(new byte[2]);

            var bitsWritten = new List<bool>();
            var bytesWritten = new List<byte[]>();
            
            mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
            mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
            _serializer.Encode(mock.Object, true);
            
            Assert.False(bitsWritten[0]);
            Assert.False(bitsWritten[1]);
            Assert.True(bitsWritten[2]);
        }
        
        [Fact]
        public void Encode_False_WritesToDataBuffer()
        {
            var mock = new Mock<DataBuffer>(new byte[2]);

            var bitsWritten = new List<bool>();
            var bytesWritten = new List<byte[]>();
            
            mock.Setup(m => m.WriteBit(Capture.In(bitsWritten)));
            mock.Setup(m => m.WriteBytes(Capture.In(bytesWritten)));
            
            _serializer.Encode(mock.Object, false);
            
            Assert.False(bitsWritten[0]);
            Assert.False(bitsWritten[1]);
            Assert.False(bitsWritten[2]);
        }

        #endregion

        #region Test Methods (Decode)

        [Fact]
        public void Decode_DataBufferWithSerializedtrueValue_ReturnsHeaderWithDecodedValue()
        {
            var header = new Header() { IsMissing = false, IsNull = false };
            var dataBuffer = new DataBuffer(new byte[2]);

            dataBuffer.WriteBit(false);
            dataBuffer.WriteBit(false);
            dataBuffer.WriteBit(true);

            HeaderWithValue<bool> headerWithValue = _serializer.Decode(dataBuffer);
            
            Assert.Equal(header, headerWithValue.Header);
            Assert.True(headerWithValue.Value);
        }
        
        [Fact]
        public void Decode_DataBufferWithSerializedFalseValue_ReturnsHeaderWithDecodedValue()
        {
            var header = new Header() { IsMissing = false, IsNull = false };
            var dataBuffer = new DataBuffer(new byte[2]);

            dataBuffer.WriteBit(false);
            dataBuffer.WriteBit(false);
            dataBuffer.WriteBit(false);

            HeaderWithValue<bool> headerWithValue = _serializer.Decode(dataBuffer);

            Assert.Equal(header, headerWithValue.Header);
            Assert.True(headerWithValue.Value);
        }

        #endregion
    }
}