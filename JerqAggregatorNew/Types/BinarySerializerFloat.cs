namespace JerqAggregatorNew.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        public override int Size => sizeof(float);
        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(float? value)
        {
            return Size;
        }

        protected override float DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToSingle(bytes);
        }
    }
}