namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt32 : BinarySerializerNumeric<int>
    {
        public override int Size => sizeof(int);

        protected override byte[] ConvertToByteArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }
    }
}