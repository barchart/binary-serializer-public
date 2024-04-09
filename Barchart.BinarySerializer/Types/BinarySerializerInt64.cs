namespace Barchart.BinarySerializer.Types
{
    public static class Int64Helper
    {
        public static int GetSizeOfInt64()
        {
            return sizeof(long);
        }

        public static byte[] ConvertInt64ToByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public static long ConvertBytesToInt64(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes);
        }
    }

    public class BinarySerializerInt64 : BinarySerializerNumeric<long>
    {
        public override int Size => Int64Helper.GetSizeOfInt64();

        protected override byte[] ConvertToByteArray(long value)
        {
            return Int64Helper.ConvertInt64ToByteArray(value);
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return Int64Helper.ConvertBytesToInt64(bytes);
        }
    }

    public class BinarySerializerInt64Nullable : BinarySerializerNullableNumeric<long>
    {
        public override int Size => Int64Helper.GetSizeOfInt64();

        protected override byte[] ConvertToByteArray(long value)
        {
            return Int64Helper.ConvertInt64ToByteArray(value);
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return Int64Helper.ConvertBytesToInt64(bytes);
        }
    }
}