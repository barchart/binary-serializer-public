#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerLong: IBinaryTypeSerializer<long>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(long);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, long value)
        {
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public long Decode(IDataBufferReader dataBuffer)
        {
            return BitConverter.ToInt64(dataBuffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(long value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}