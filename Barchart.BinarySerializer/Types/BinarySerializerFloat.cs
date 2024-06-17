namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        #region Properties

        public override int Size => sizeof(float);

        #endregion

        #region Methods

        protected override byte[] ConvertToByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }

        #endregion
    }
}