namespace Barchart.BinarySerializer.SerializationUtilities.Types
{
    public class BinarySerializerSbyte : BinarySerializerNumeric<sbyte>
    {
        #region Properties

        public override int Size => sizeof(sbyte);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }

        #endregion
    }
}