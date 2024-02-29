namespace BinarySerializer.Types
{
    public class BinarySerializerSInt8 : BinarySerializerNumeric<sbyte>
    {
        public override int Size => sizeof(sbyte);

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        public override int GetLengthInBits(sbyte? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
    }
}