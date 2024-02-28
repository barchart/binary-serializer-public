namespace JerqAggregatorNew.Types
{
    public class BinarySerializerBool8 : BinarySerializerNumeric<bool>
    {
        public override int Size => sizeof(bool);

        protected override byte[] ConvertToByteArray(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(bool? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return (bytes[0] & 0x01) != 0;
        }
    }
}