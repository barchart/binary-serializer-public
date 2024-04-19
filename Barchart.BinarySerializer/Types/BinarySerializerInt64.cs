namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt64 : BinarySerializerNumeric<long>
    {
        public override int Size => sizeof(long);

        protected override byte[] ConvertToByteArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes);
        }
    }
}