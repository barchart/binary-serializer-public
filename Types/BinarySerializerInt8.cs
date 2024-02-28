namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt8 : BinarySerializerNumeric<byte>
    {
        public override int Size => sizeof(byte);

        protected override byte[] ConvertToByteArray(byte value)
        {
            return new byte[] { value };
        }

        public override int GetLengthInBits(byte? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }
    }
}