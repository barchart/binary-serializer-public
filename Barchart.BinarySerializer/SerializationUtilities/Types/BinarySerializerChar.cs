namespace Barchart.BinarySerializer.DataSerialization.Types
{
    public class BinarySerializerChar : BinarySerializerNumeric<char>
    {
        #region Properties

        public override int Size => sizeof(char);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(char value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }

        #endregion
    }
}