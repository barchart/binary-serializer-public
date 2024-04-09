namespace Barchart.BinarySerializer.Types
{
    public static class FloatHelper
    {
        public static int GetSizeOfFloat()
        {
            return sizeof(float);
        }

        public static byte[] ConvertFloatToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public static float ConvertBytesToFloat(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
    }

    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        public override int Size => FloatHelper.GetSizeOfFloat();

        protected override byte[] ConvertToByteArray(float value)
        {
            return FloatHelper.ConvertFloatToByteArray(value);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return FloatHelper.ConvertBytesToFloat(bytes);
        }
    }

    public class BinarySerializerFloatNullable : BinarySerializerNullableNumeric<float>
    {
        public override int Size => FloatHelper.GetSizeOfFloat();

        protected override byte[] ConvertToByteArray(float value)
        {
            return FloatHelper.ConvertFloatToByteArray(value);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return FloatHelper.ConvertBytesToFloat(bytes);
        }
    }
}