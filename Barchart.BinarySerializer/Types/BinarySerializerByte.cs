using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerByte : BinarySerializerNumeric<byte>
    {
        #region Properties
        
        public override int Size => sizeof(byte);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, byte value)
        {
            dataBuffer.WriteByte(value);
        }

        protected override byte DecodeBytes(byte[] bytes)
        {
            return bytes[0];
        }

        #endregion
    }
}