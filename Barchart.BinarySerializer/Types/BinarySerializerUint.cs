namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUint : BinarySerializerNumeric<uint>
    {
        #region Properties

        public override int Size => sizeof(uint);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }

        #endregion
    }
}