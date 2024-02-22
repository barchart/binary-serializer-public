namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        public override int Size => sizeof(double);

        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(double? value)
        {
            return Size;
        }

        protected override double DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToDouble(bytes);
        }
    }
}