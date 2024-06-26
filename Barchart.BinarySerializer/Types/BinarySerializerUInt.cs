#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt : IBinaryTypeSerializer<uint>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(uint);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, uint value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public uint Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToUInt32(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(uint value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}