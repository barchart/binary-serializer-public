namespace Barchart.BinarySerializer.Types
{
    public static class UInt16Helper
    {
        public static int GetSizeOfUInt16()
        {
            return sizeof(ushort);
        }

        public static byte[] ConvertUInt16ToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public static ushort ConvertBytesToUInt16(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }

    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        public override int Size => UInt16Helper.GetSizeOfUInt16();

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return UInt16Helper.ConvertUInt16ToByteArray(value);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return UInt16Helper.ConvertBytesToUInt16(bytes);
        }
    }

    public class BinarySerializerUInt16Nullable : BinarySerializerNullableNumeric<ushort>
    {
        public override int Size => UInt16Helper.GetSizeOfUInt16();

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return UInt16Helper.ConvertUInt16ToByteArray(value);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return UInt16Helper.ConvertBytesToUInt16(bytes);
        }
    }
}