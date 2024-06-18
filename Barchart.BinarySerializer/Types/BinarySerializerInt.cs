namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt : BinarySerializerNumeric<int>
    {
        #region Properties

        public override int Size => sizeof(int);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }

        #endregion
    }
}