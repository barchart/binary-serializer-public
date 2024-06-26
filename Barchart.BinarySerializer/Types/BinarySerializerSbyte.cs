#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerSbyte : IBinaryTypeSerializer<sbyte>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BYTES = sizeof(long);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, sbyte value)
        {
            dataBuffer.WriteByte((byte)value);
        }

        /// <inheritdoc />
        public sbyte Decode(IDataBufferReader dataBuffer)
        {
            return (sbyte)dataBuffer.ReadByte();
        }

        /// <inheritdoc />
        public int GetLengthInBits(sbyte value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        #endregion
    }
}