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
        public void Encode(IDataBufferWriter buffer, uint value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public uint Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToUInt32(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(uint value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        public bool GetEquals(uint a, uint b)
        {
            return a.Equals(b);
        }
        
        #endregion
    }
}