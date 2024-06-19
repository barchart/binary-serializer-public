namespace Barchart.BinarySerializer.DataSerialization.Types
{
    public class BinarySerializerByte : BinarySerializerNumeric<byte>
    {
        #region Properties
        
        public override int Size => sizeof(byte);

        #endregion

        #region  Methods

        protected override byte[] ConvertToByteArray(byte value)
        {
            return new byte[] { value };
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }

        #endregion
    }
}