namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        public override int Size => sizeof(ushort);

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(ushort? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }
}