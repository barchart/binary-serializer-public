#region Using Statements

using Barchart.BinarySerializer.Tests.Utility;

#endregion

namespace Barchart.BinarySerializer.Tests.SerializationUtilities.Types
{
    public class BinarySerializerBoolTests
    {
        #region Fields
    
        private readonly ITestOutputHelper _testOutputHelper;
        
        #endregion
        
        #region Constructor(s)
        
        public BinarySerializerBoolTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
    
        #endregion

        #region Test Methods (ConvertToByteArray)
        [Fact]
        public void ConvertToByteArray_True_ReturnsOneByteArrayWithOneAtTheEnd()
        {
            var serializer = new BinarySerializerBoolTest();
            bool value = true;

            byte[] result = serializer.ConvertToByteArray(value);

            Assert.Equal(new byte[] { 1 }, result);
        }


        [Fact]
        public void ConvertToByteArray_False_ReturnsByteArrayWithZeroAtTheEnd()
        {
            var serializer = new BinarySerializerBoolTest();
            bool value = false;

            byte[] result = serializer.ConvertToByteArray(value);

            Assert.Equal(new byte[] { 0 }, result);
        }

        #endregion

        #region Test Methods (DecodeBytes)
        [Fact]
        public void DecodeBytes_ArrayWithOneAtTheEnd_ReturnsTrueIfValueIsOne()
        {
            var serializer = new BinarySerializerBoolTest();
            byte[] decodedBytes = new byte[] { 1 };

            bool value = serializer.DecodeBytes(decodedBytes);

            Assert.True(value);
        }

        [Fact]
        public void DecodeBytes_ArrayWithZeroAtTheEnd_ReturnsTrueIfValueIsZero()
        {
            var serializer = new BinarySerializerBoolTest();
            byte[] decodedBytes = new byte[] { 0 };

            bool value = serializer.DecodeBytes(decodedBytes);

            Assert.False(value);
        }

        #endregion
    }
}