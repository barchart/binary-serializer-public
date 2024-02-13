namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt8 : BinarySerializerNumeric<byte>
    {
        protected override int Size => sizeof(byte);
        protected override byte[] ConvertToByteArray(byte value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(byte? value)
        {
            return Size;
        }

        protected override byte DecodeBytes(byte[] bytes, int offset)
        {
            return bytes[offset];
        }
    }
}
