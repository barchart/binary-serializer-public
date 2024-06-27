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
        public void Encode(IDataBufferWriter buffer, ushort value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public ushort Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToUInt16(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(ushort value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        public bool GetEquals(ushort a, ushort b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}