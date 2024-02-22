namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt64 : BinarySerializerNumeric<long>
    {
        public override int Size => sizeof(long);

        protected override byte[] ConvertToByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(long? value)
        {
            return Size;
        }

        protected override long DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToInt64(bytes);
        }
    }
}