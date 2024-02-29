namespace BinarySerializer.Types
{
    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
    {
        public override int Size => sizeof(short);

        protected override byte[] ConvertToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(short? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }
    }
}