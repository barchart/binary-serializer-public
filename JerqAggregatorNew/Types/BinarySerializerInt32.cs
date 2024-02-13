namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt32 : BinarySerializerNumeric<int>
    {
        protected override int Size => sizeof(int); 
        protected override byte[] ConvertToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(int? value)
        {
            return Size;
        }

        protected override int DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToInt32(bytes);
        }
    }
}
