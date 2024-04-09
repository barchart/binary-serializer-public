namespace Barchart.BinarySerializer.Types
{
    public static class Int16Helper
    {
        public static int GetSizeOfInt16()
        {
            return sizeof(short);
        }

        public static byte[] ConvertInt16ToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static short ConvertBytesToInt16(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }
    }

    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
    {
        public override int Size => Int16Helper.GetSizeOfInt16();

        protected override byte[] ConvertToByteArray(short value)
        {
            return Int16Helper.ConvertInt16ToByteArray(value);
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return Int16Helper.ConvertBytesToInt16(bytes);
        }
    }

    public class BinarySerializerInt16Nullable : BinarySerializerNullableNumeric<short>
    {
        public override int Size => Int16Helper.GetSizeOfInt16();

        protected override byte[] ConvertToByteArray(short value)
        {
            return Int16Helper.ConvertInt16ToByteArray(value);
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return Int16Helper.ConvertBytesToInt16(bytes);
        }
    }
}