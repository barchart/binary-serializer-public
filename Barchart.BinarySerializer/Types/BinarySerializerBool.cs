using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerBool : BinarySerializerNumeric<bool>
    {
        #region Properties

        public override int Size => sizeof(bool);

        #endregion

        #region Methods
        
        protected override void EncodeValue(IDataBuffer dataBuffer, bool value)
        {
            dataBuffer.WriteBit(value);
        }

        protected override bool DecodeBytes(byte[] bytes)
        {
            return bytes[0] == 1;
        }

        #endregion
    }
}