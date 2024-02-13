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

        public override int GetLengthInBytes(decimal? value)
        {
            return Size;
        }

        protected override decimal DecodeBytes(byte[] bytes, int offset)
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