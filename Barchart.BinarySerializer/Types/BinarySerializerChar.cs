using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerChar : BinarySerializerNumeric<char>
    {
        #region Properties

        public override int Size => sizeof(char);

        #endregion

        #region Methods

        protected override void EncodeValue(IDataBuffer dataBuffer, char value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        protected override char DecodeBytes(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }

        #endregion
    }
}