namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        #region Properties

        public override int Size => sizeof(ushort);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }

        #endregion
    }
}