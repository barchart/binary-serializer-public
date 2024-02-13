namespace JerqAggregatorNew.Types
{
    public class BinarySerializerBool8 : BinarySerializerNumeric<bool>
    {
        public override int Size => sizeof(bool);
        protected override byte[] ConvertToByteArray(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(bool? value)
        {
            return Size;
        }

        protected override bool DecodeBytes(byte[] bytes, int offset)
        {
            return (bytes[offset] & 0x01) != 0;
        }
    }
}
