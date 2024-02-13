namespace JerqAggregatorNew.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        protected override int Size => sizeof(float);
        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(float? value)
        {
            return Size;
        }

        protected override float DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToSingle(bytes);
        }
    }
}
