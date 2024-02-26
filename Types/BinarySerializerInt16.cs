namespace JerqAggregatorNew.Types
{
    public class BinarySerializerInt16 : BinarySerializerNumeric<short>
    {
        public override int Size => sizeof(short);

        protected override byte[] ConvertToByteArray(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(short? value)
        {
            return Size;
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }
    }
}