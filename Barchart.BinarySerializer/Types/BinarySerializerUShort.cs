#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUShort : IBinaryTypeSerializer<ushort>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(ushort);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, ushort value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public ushort Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToUInt16(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(ushort value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}