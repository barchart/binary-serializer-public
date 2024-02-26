namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt8 : BinarySerializerNumeric<byte>
    {
        public override int Size => sizeof(byte);

        protected override byte[] ConvertToByteArray(byte value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(byte? value)
        {
            return Size;
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }
    }
}