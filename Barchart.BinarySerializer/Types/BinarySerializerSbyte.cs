using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerSbyte : BinarySerializerNumeric<sbyte>
    {
        #region Properties

        public override int Size => sizeof(sbyte);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, sbyte value)
        {
            dataBuffer.WriteBytes( new byte[] { (byte)value });
        }

        protected override sbyte DecodeBytes(byte[] bytes)
        {
            return (sbyte)bytes[0];
        }
        
        #endregion
    }
}