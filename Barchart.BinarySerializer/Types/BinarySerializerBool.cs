namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerBool : BinarySerializerNumeric<bool>
    {
        #region Properties

        public override int Size => sizeof(bool);

        #endregion

        #region Methods
        
        protected override byte[] ConvertToByteArray(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return bytes[0] == 1;
        }

        #endregion
    }
}