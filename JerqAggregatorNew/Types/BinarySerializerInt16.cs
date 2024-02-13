namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
    {
        protected override int Size => sizeof(short);
        protected override byte[] ConvertToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(short? value)
        {
            return Size;
        }

        protected override short DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToInt16(bytes);
        }
    }
}
