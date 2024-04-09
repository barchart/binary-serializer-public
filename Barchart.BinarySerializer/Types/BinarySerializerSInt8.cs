namespace Barchart.BinarySerializer.Types
{
    public static class SInt8Helper
    {
        public static int GetSizeOfSInt8()
        {
            return sizeof(sbyte);
        }

        public static byte[] ConvertSInt8ToByteArray(sbyte value)
        {
            return new byte[] { (byte)value };
        }

        public static sbyte ConvertBytesToSInt8(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
    }

    public class BinarySerializerSInt8 : BinarySerializerNumeric<sbyte>
    {
        public override int Size => SInt8Helper.GetSizeOfSInt8();

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return SInt8Helper.ConvertSInt8ToByteArray(value);
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return SInt8Helper.ConvertBytesToSInt8(bytes);
        }
    }

    public class BinarySerializerSInt8Nullable : BinarySerializerNullableNumeric<sbyte>
    {
        public override int Size => SInt8Helper.GetSizeOfSInt8();

        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return SInt8Helper.ConvertSInt8ToByteArray(value);
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return SInt8Helper.ConvertBytesToSInt8(bytes);
        }
    }
}