namespace Barchart.BinarySerializer.Types
{
    public static class DoubleHelper
    {
        public static int GetSizeOfDouble()
        {
            return sizeof(double);
        }

        public static byte[] ConvertDoubleToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public static double ConvertBytesToDouble(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes);
        }
    }

    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        public override int Size => DoubleHelper.GetSizeOfDouble();

        protected override byte[] ConvertToByteArray(double value)
        {
            return DoubleHelper.ConvertDoubleToByteArray(value);
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return DoubleHelper.ConvertBytesToDouble(bytes);
        }
    }

    public class BinarySerializerDoubleNullable : BinarySerializerNullableNumeric<double>
    {
        public override int Size => DoubleHelper.GetSizeOfDouble();

        protected override byte[] ConvertToByteArray(double value)
        {
            return DoubleHelper.ConvertDoubleToByteArray(value);
        }

        protected override double DecodeBytes(byte[] bytes)
        {
            return DoubleHelper.ConvertBytesToDouble(bytes);
        }
    }
}