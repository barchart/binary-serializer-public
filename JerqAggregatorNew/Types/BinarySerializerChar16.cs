namespace JerqAggregatorNew.Types
{
    public class BinarySerializerChar16 : BinarySerializerNumeric<char>
    {
        protected override int Size => sizeof(char);
        protected override byte[] ConvertToByteArray(char value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(char? value)
        {
            return Size;
        }

        protected override char DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToChar(bytes);
        }
    }
}
