using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUlong : BinarySerializerNumeric<ulong>
    {
        #region Properties

        public override int Size => sizeof(ulong);

        #endregion

        #region Methods

        protected override void EncodeValue(DataBuffer dataBuffer, ulong value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }


        protected override ulong DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes);
        }

        #endregion
    }
}