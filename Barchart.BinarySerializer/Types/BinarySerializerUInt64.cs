namespace Barchart.BinarySerializer.Types
{
    public static class UInt64Helper
    {
        public static int GetSizeOfUInt64()
        {
            return sizeof(ulong);
        }

        public static byte[] ConvertUInt64ToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        public static ulong ConvertBytesToUInt64(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }

    public class BinarySerializerUInt64 : BinarySerializerNumeric<ulong>
    {
        public override int Size => UInt64Helper.GetSizeOfUInt64();

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return UInt64Helper.ConvertUInt64ToByteArray(value);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return UInt64Helper.ConvertBytesToUInt64(bytes);
        }
    }

    public class BinarySerializerUInt64Nullable : BinarySerializerNullableNumeric<ulong>
    {
        public override int Size => UInt64Helper.GetSizeOfUInt64();

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return UInt64Helper.ConvertUInt64ToByteArray(value);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return UInt64Helper.ConvertBytesToUInt64(bytes);
        }
    }
}