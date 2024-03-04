namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
    {
        public override int Size => sizeof(short);

        protected override byte[] ConvertToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }
    }
}