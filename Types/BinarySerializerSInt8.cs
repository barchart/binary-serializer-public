namespace JerqAggregatorNew.Types
{
    public class BinarySerializerSInt8 : BinarySerializerNumeric<sbyte>
    {
        public override int Size => sizeof(sbyte);

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        public override int GetLengthInBytes(sbyte? value)
        {
            return Size;
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
    }
}