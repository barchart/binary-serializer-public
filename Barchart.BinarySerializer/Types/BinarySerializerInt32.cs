namespace Barchart.BinarySerializer.Types
{
    public static class Int32Helper
    {
        public static int GetSizeOfInt32()
        {
            return sizeof(int);
        }

        public static byte[] ConvertInt32ToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static int ConvertBytesToInt32(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }
    }

    public class BinarySerializerInt32 : BinarySerializerNumeric<int>
    {
        public override int Size => Int32Helper.GetSizeOfInt32();

        protected override byte[] ConvertToByteArray(int value)
        {
            return Int32Helper.ConvertInt32ToByteArray(value);
        }

        protected override int DecodeBytes(byte[] bytes)
        {
            return Int32Helper.ConvertBytesToInt32(bytes);
        }
    }

    public class BinarySerializerInt32Nullable : BinarySerializerNullableNumeric<int>
    {
        public override int Size => Int32Helper.GetSizeOfInt32();

        protected override byte[] ConvertToByteArray(int value)
        {
            return Int32Helper.ConvertInt32ToByteArray(value);
        }

        protected override int DecodeBytes(byte[] bytes)
        {
            return Int32Helper.ConvertBytesToInt32(bytes);
        }
    }
}