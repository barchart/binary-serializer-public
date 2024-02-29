namespace BinarySerializer.Types
{
    public class BinarySerializerChar16 : BinarySerializerNumeric<char>
    {
        public override int Size => sizeof(char);

        protected override byte[] ConvertToByteArray(char value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(char? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }
    }
}