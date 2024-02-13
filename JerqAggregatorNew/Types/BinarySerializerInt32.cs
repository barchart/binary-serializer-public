namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt32 : BinarySerializerNumeric<int>
    {
        public override int Size => sizeof(int);
        protected override byte[] ConvertToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(int? value)
        {
            return Size;
        }

        protected override int DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToInt32(bytes);
        }
    }
}