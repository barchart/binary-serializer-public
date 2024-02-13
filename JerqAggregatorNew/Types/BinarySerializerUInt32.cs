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
            return Size;
        }

        protected override uint DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToUInt32(bytes);
        }
    }
}
