namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt32 : BinarySerializerNumeric<uint>
    {
        public override int Size => sizeof(int);

        protected override byte[] ConvertToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(uint? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }
}