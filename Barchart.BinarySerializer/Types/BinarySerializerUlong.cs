namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUlong : BinarySerializerNumeric<ulong>
    {
        #region Properties

        public override int Size => sizeof(ulong);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }

        #endregion
    }
}