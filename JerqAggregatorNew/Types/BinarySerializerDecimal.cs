namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDecimal : BinarySerializerNumeric<decimal>
    {
        protected override int Size => sizeof(decimal);
        protected override byte[] ConvertToByteArray(decimal value)
        {
            return Decimal.GetBits(value)
                        .SelectMany(BitConverter.GetBytes)
                        .ToArray();
        }

        protected override int GetLengthInBytes(decimal? value)
        {
            return Size;
        }

        protected override decimal DecodeBytes(byte[] bytes, int offset)
        {
            int[] bits = new int[4];
            Buffer.BlockCopy(bytes, 0, bits, 0, sizeof(decimal));
            return new Decimal(bits);
        }
    }
}
