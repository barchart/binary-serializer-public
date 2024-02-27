namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt64 : BinarySerializerNumeric<ulong>
    {
        public override int Size => sizeof(ulong);

        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(ulong? value)
        {
            return Size + sizeof(byte);
        }

        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }
}