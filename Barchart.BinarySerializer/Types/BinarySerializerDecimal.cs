namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerDecimal : BinarySerializerNumeric<decimal>
    {
        public override int Size => sizeof(decimal);

        protected override byte[] ConvertToByteArray(decimal value)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            writer.Write(value);

            return stream.ToArray();
        }

        protected override decimal DecodeBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            using BinaryReader reader = new(stream);

            return reader.ReadDecimal();
        }
    }
}