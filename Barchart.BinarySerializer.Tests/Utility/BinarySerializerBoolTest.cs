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
            if (bytes.Length != Size)
            {
                throw new ArgumentException("Invalid byte array length for decoding boolean value.");
            }
            
            return base.DecodeBytes(bytes);
        }

        #endregion
    }
}