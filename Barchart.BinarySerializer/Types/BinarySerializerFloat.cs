namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        public override int Size => sizeof(float);

        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
    }

    public class BinarySerializerFloatNullable : BinarySerializerNullableNumeric<float>
    {
        public override int Size => sizeof(float);

        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }
    }
}