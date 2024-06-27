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
        public void Encode(IDataBufferWriter buffer, long value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public long Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToInt64(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(long value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(long a, long b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}