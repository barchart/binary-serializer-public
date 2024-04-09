namespace Barchart.BinarySerializer.Types
{
    public static class BoolHelper
    {
        public static int GetSizeOfBool()
        {
            return sizeof(bool);
        }

        public static byte[] ConvertToByteArray(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public static bool DecodeBytes(byte[] bytes)
        {
            return bytes[0] == 1;
        }
    }

    public class BinarySerializerBool8 : BinarySerializerNumeric<bool>
    {
        public override int Size => BoolHelper.GetSizeOfBool();

        protected override byte[] ConvertToByteArray(bool value)
        {
            return BoolHelper.ConvertToByteArray(value);
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return BoolHelper.DecodeBytes(bytes);
        }
    }

    public class BinarySerializerBool8Nullable : BinarySerializerNullableNumeric<bool>
    {
        public override int Size => BoolHelper.GetSizeOfBool();

        protected override byte[] ConvertToByteArray(bool value)
        {
            return BoolHelper.ConvertToByteArray(value);
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return BoolHelper.DecodeBytes(bytes);
        }
    }
}