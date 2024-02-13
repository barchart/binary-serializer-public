namespace JerqAggregatorNew.Types
{
    public class BinarySerializerUInt16 : BinarySerializerNumeric<ushort>
    {
        protected override int Size => sizeof(ushort);
        protected override byte[] ConvertToByteArray(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override int GetLengthInBytes(ushort? value)
        {
            return Size;
        }

        protected override ushort DecodeBytes(byte[] bytes, int offset)
        {
            return BitConverter.ToUInt16(bytes);
        }
    }
}
