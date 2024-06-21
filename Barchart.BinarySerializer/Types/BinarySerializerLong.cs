using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerLong: BinarySerializerNumeric<long>
    {
        #region Properties

        public override int Size => sizeof(long);

        #endregion

        #region Methods

        protected override void EncodeValue(DataBuffer dataBuffer, long value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override long DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes);
        }

        #endregion
    }
}