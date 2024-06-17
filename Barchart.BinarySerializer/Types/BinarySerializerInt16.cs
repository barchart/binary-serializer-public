namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
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