namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt64 : BinarySerializerNumeric<ulong>
    {
        protected override int Size => sizeof(ulong);
        protected override byte[] ConvertToByteArray(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(ulong? value)
        {
            return Size;
        }

        protected override ulong DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToUInt64(bytes);
        }
    }
}
