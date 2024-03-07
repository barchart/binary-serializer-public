namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        public override int Size => sizeof(ushort);

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }

    public class BinarySerializerUInt16Nullable : BinarySerializerNullableNumeric<ushort>
    {
        public override int Size => sizeof(ushort);

        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override ushort DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }
}