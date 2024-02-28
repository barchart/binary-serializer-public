namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt64 : BinarySerializerNumeric<ulong>
    {
        public override int Size => sizeof(ulong);

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(ulong? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }
}