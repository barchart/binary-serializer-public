namespace JerqAggregatorNew.Types
{
    public class BinarySerializerSInt8 : BinarySerializerNumeric<sbyte>
    {
        public override int Size => sizeof(sbyte);
        protected override byte[] ConvertToByteArray(sbyte value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(sbyte? value)
        {
            return Size;
        }

        protected override sbyte DecodeBytes(byte[] bytes, int offset)
        {
            return (sbyte)bytes[offset];
        }
    }
}