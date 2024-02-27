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
            return Size + sizeof(byte);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
    }
}