namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt8 : BinarySerializerNumeric<byte>
    {
        public override int Size => sizeof(byte);

        protected override byte[] ConvertToByteArray(byte value)
        {
            return new byte[] { value };
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }
    }

    public class BinarySerializerInt8Nullable : BinarySerializerNullableNumeric<byte>
    {
        public override int Size => sizeof(byte);

        protected override byte[] ConvertToByteArray(byte value)
        {
            return new byte[] { value };
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }
    }
}