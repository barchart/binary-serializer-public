namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt32 : BinarySerializerNumeric<int>
    {
        public override int Size => sizeof(int);

        protected override byte[] ConvertToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(int? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override int DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }
    }
}