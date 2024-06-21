using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        #region Properties

        public override int Size => sizeof(float);

        #endregion

        #region Methods

        protected override void EncodeValue(DataBuffer dataBuffer, float value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override float DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes);
        }

        #endregion
    }
}