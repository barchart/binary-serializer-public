namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        public override int Size => sizeof(double);

        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(double? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }
    }
}