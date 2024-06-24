using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUint : BinarySerializerNumeric<uint>
    {
        #region Properties

        public override int Size => sizeof(uint);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, uint value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override uint DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes);
        }

        #endregion
    }
}