#region Using Statements

using Barchart.BinarySerializer.SerializationUtilities.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Utility {
 public class BinarySerializerBoolTest : BinarySerializerBool
    {
        #region Methods
        
        public new byte[] ConvertToByteArray(bool value)
        {
            return base.ConvertToByteArray(value);
        }

        public new bool DecodeBytes(byte[] bytes)
        {
            return base.DecodeBytes(bytes);
        }

        #endregion
    }
}