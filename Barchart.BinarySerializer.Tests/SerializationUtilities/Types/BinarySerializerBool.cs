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

        #endregion
    }
}