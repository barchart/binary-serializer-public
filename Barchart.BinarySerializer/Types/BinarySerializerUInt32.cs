namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt32 : BinarySerializerNumeric<uint>
    {
        public override int Size => sizeof(uint);

        protected override byte[] ConvertToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }

    public class BinarySerializerUInt32Nullable : BinarySerializerNullableNumeric<uint>
    {
        public override int Size => sizeof(uint);

        protected override byte[] ConvertToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }
}