namespace Barchart.BinarySerializer.Types
{
    public static class DecimalHelper
    {
        public static int GetSizeOfDecimal()
        {
            return sizeof(decimal);
        }

        public static byte[] ConvertDecimalToByteArray(decimal value)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            writer.Write(value);

            return stream.ToArray();
        }

        public static decimal ConvertBytesToDecimal(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            using BinaryReader reader = new(stream);

            return reader.ReadDecimal();
        }
    }

    public class BinarySerializerDecimal : BinarySerializerNumeric<decimal>
    {
        public override int Size => DecimalHelper.GetSizeOfDecimal();

        protected override byte[] ConvertToByteArray(decimal value)
        {
            return DecimalHelper.ConvertDecimalToByteArray(value);
        }

        protected override decimal DecodeBytes(byte[] bytes)
        {
            return DecimalHelper.ConvertBytesToDecimal(bytes);
        }
    }

    public class BinarySerializerDecimalNullable : BinarySerializerNullableNumeric<decimal>
    {
        public override int Size => DecimalHelper.GetSizeOfDecimal();

        protected override byte[] ConvertToByteArray(decimal value)
        {
            return DecimalHelper.ConvertDecimalToByteArray(value);
        }

        protected override decimal DecodeBytes(byte[] bytes)
        {
            return DecimalHelper.ConvertBytesToDecimal(bytes);
        }
    }
}