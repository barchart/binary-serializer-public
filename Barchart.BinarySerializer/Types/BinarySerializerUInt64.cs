namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt64 : BinarySerializerNumeric<ulong>
    {
        public override int Size => sizeof(ulong);

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }

    public class BinarySerializerUInt64Nullable : BinarySerializerNullableNumeric<ulong>
    {
        public override int Size => sizeof(ulong);

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }
}