using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerShort : BinarySerializerNumeric<short>
    {
        #region Properties

        public override int Size => sizeof(short);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, short value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override short DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes);
        }

        #endregion
    }
}