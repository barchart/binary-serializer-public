using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerFloat : BinarySerializerNumeric<float>
    {
        #region Properties

        public override int Size => sizeof(float);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, float value)
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