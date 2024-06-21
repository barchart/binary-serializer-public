#region Using Statements

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

        #region Test Methods (ConvertToByteArray)
        
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
            
            Assert.Single(bytesWritten);
            Assert.Equal(1, bytesWritten[0][0]);
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
            
            Assert.Single(bytesWritten);
            Assert.Equal(0, bytesWritten[0][0]);
        }

        #endregion
    }
}