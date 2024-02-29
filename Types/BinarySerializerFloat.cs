namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        public override int Size => sizeof(float);

        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBits(float? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
    }
}