#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;
using static Barchart.BinarySerializer.Tests.Common.Helpers;

#endregion

namespace Barchart.BinarySerializer.Tests.Types
{
    public class BinarySerializerEnumTests
    {
      

        #region Fields
    
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly BinarySerializerEnum<TestEnum> _serializer;
    
        #endregion
        
        #region Constructor(s)
        
        public BinarySerializerEnumTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _serializer = new BinarySerializerEnum<TestEnum>(new BinarySerializerInt());
        }
    
        #endregion

        #region Test Methods (Encode)
        
        [Theory]
        [InlineData(TestEnum.Value1)]
        [InlineData(TestEnum.Value2)]
        [InlineData(TestEnum.Value3)]
        public void Encode_Various_WritesExpectedBytes(TestEnum value)
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
        
            int intValue = Convert.ToInt32(value);
            byte[] expectedBytes = BitConverter.GetBytes(intValue);
        
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
        [InlineData(TestEnum.Value1)]
        [InlineData(TestEnum.Value2)]
        [InlineData(TestEnum.Value3)]
        public void Decode_VariousEncoded_ReturnsExpectedValue(TestEnum expectedValue)
        {
            int intValue = Convert.ToInt32(expectedValue);

            Mock<IDataBufferReader> mock = new();
            mock.Setup(m => m.ReadBytes(4)).Returns(BitConverter.GetBytes(intValue));

            TestEnum actualValue = _serializer.Decode(mock.Object);

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion
    
        #region Test Methods (GetEquals)
    
        [Theory]
        [InlineData(new[] { TestEnum.Value1, TestEnum.Value1 })]
        [InlineData(new[] { TestEnum.Value2, TestEnum.Value2 })]
        [InlineData(new[] { TestEnum.Value3, TestEnum.Value3 })]
        [InlineData(new[] { TestEnum.Value1, TestEnum.Value2 })]
        [InlineData(new[] { TestEnum.Value2, TestEnum.Value3 })]
        [InlineData(new[] { TestEnum.Value1, TestEnum.Value3 })]
        public void GetEquals_Various_ReturnsExpectedResult(TestEnum[] testEnums)
        {
            var actual = _serializer.GetEquals(testEnums[0], testEnums[1]);
            var expected = testEnums[0].Equals(testEnums[1]);

            Assert.Equal(expected, actual);
        }
    
        #endregion
    }
}