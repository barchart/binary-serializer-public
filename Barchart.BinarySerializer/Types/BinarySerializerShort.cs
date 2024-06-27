#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerShort : IBinaryTypeSerializer<short>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(short);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion
        
        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter buffer, short value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public short Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToInt16(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(short value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        public bool GetEquals(short a, short b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}