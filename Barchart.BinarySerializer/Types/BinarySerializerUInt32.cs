namespace Barchart.BinarySerializer.Types
{
    public static class UInt32Helper
    {
        public static int GetSizeOfUInt32()
        {
            return sizeof(uint);
        }

        public static byte[] ConvertUInt32ToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public static uint ConvertBytesToUInt32(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }

    public class BinarySerializerUInt32 : BinarySerializerNumeric<uint>
    {
        public override int Size => UInt32Helper.GetSizeOfUInt32();

        protected override byte[] ConvertToByteArray(uint value)
        {
            return UInt32Helper.ConvertUInt32ToByteArray(value);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return UInt32Helper.ConvertBytesToUInt32(bytes);
        }
    }

    public class BinarySerializerUInt32Nullable : BinarySerializerNullableNumeric<uint>
    {
        public override int Size => UInt32Helper.GetSizeOfUInt32();

        protected override byte[] ConvertToByteArray(uint value)
        {
            return UInt32Helper.ConvertUInt32ToByteArray(value);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return UInt32Helper.ConvertBytesToUInt32(bytes);
        }
    }
}