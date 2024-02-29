namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt64 : BinarySerializerNumeric<long>
    {
        public override int Size => sizeof(long);

        protected override byte[] ConvertToByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(long? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes);
        }
    }
}