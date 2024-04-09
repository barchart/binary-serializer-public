namespace Barchart.BinarySerializer.Types
{
    public static class CharHelper
    {
        public static int GetSizeOfChar()
        {
            return sizeof(char);
        }

        public static byte[] ConvertToByteArray(char value)
        {
            return BitConverter.GetBytes(value);
        }

        public static char DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }
    }

    public class BinarySerializerChar16 : BinarySerializerNumeric<char>
    {
        public override int Size => CharHelper.GetSizeOfChar();

        protected override byte[] ConvertToByteArray(char value)
        {
            return CharHelper.ConvertToByteArray(value);
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return CharHelper.DecodeBytes(bytes);
        }
    }

    public class BinarySerializerChar16Nullable : BinarySerializerNullableNumeric<char>
    {
        public override int Size => CharHelper.GetSizeOfChar();

        protected override byte[] ConvertToByteArray(char value)
        {
            return CharHelper.ConvertToByteArray(value);
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return CharHelper.DecodeBytes(bytes);
        }
    }
}