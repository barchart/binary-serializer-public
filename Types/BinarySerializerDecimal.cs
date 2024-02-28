namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDecimal : BinarySerializerNumeric<decimal>
    {
        public override int Size => sizeof(decimal);

        protected override byte[] ConvertToByteArray(decimal value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return (stream.ToArray());
                }
            }
        }

        public override int GetLengthInBits(decimal? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NUMERIC;
            }

            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected override decimal DecodeBytes(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return (reader.ReadDecimal());
                }
            }
        }
    }
}