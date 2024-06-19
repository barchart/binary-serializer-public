namespace Barchart.BinarySerializer.SerializationUtilities.Types
{
    public class BinarySerializerShort : BinarySerializerNumeric<short>
    {
        #region Properties

        public override int Size => sizeof(short);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }

        #endregion
    }
}