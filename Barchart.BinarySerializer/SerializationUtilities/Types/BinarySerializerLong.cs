namespace Barchart.BinarySerializer.DataSerialization.Types
{
    public class BinarySerializerLong: BinarySerializerNumeric<long>
    {
        #region Properties

        public override int Size => sizeof(long);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes);
        }

        #endregion
    }
}