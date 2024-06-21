using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerBool : BinarySerializerNumeric<bool>
    {
        #region Properties

        public override int Size => sizeof(bool);

        #endregion

        #region Methods
        
        protected override void EncodeValue(DataBuffer dataBuffer, bool value)
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