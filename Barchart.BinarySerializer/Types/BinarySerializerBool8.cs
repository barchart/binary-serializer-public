namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerBool8 : BinarySerializerNumeric<bool>
    {
        public override int Size => sizeof(bool);

        protected override byte[] ConvertToByteArray(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return bytes[0] == 1;
        }
    }
}