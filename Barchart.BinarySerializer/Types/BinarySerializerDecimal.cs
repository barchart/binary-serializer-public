﻿namespace Barchart.BinarySerializer.Types
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

    public class BinarySerializerDecimalNullable : BinarySerializerNullableNumeric<decimal>
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