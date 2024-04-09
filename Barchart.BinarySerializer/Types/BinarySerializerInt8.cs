namespace Barchart.BinarySerializer.Types
{
    public static class ByteHelper
    {
        public static int GetSizeOfByte()
        {
            return sizeof(byte);
        }

        public static byte[] ConvertByteToByteArray(byte value)
        {
            return new byte[] { value };
        }

        public static byte ConvertBytesToByte(byte[] bytes)
        {
            return bytes[0];
        }
    }

    public class BinarySerializerInt8 : BinarySerializerNumeric<byte>
    {
        public override int Size => ByteHelper.GetSizeOfByte();

        protected override byte[] ConvertToByteArray(byte value)
        {
            return ByteHelper.ConvertByteToByteArray(value);
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return ByteHelper.ConvertBytesToByte(bytes);
        }
    }

    public class BinarySerializerInt8Nullable : BinarySerializerNullableNumeric<byte>
    {
        public override int Size => ByteHelper.GetSizeOfByte();

        protected override byte[] ConvertToByteArray(byte value)
        {
            return ByteHelper.ConvertByteToByteArray(value);
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return ByteHelper.ConvertBytesToByte(bytes);
        }
    }
}