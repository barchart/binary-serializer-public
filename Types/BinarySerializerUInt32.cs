namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt32 : BinarySerializerNumeric<uint>
    {
        public override int Size => sizeof(int);

        protected override byte[] ConvertToByteArray(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public override int GetLengthInBytes(uint? value)
        {
            return Size + sizeof(byte);
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }
}