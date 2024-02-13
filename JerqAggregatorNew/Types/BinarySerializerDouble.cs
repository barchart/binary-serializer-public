namespace JerqAggregatorNew.Types
{
    public class BinarySerializerDouble : BinarySerializerNumeric<double>
    {
        protected override int Size => sizeof(double);
        protected override byte[] ConvertToByteArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(double? value)
        {
            return Size;
        }

        protected override double DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToDouble(bytes);
        }
    }
}
