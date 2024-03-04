namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerChar16 : BinarySerializerNumeric<char>
    {
        public override int Size => sizeof(char);

        protected override byte[] ConvertToByteArray(char value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }
    }
}