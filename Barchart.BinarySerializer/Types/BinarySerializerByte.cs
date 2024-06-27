#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) bytes to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerByte : IBinaryTypeSerializer<byte>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BITS = sizeof(byte) * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, byte value)
        {
            dataBuffer.WriteByte(value);
        }

        /// <inheritdoc />
        public byte Decode(IDataBufferReader dataBuffer)
        {
            return dataBuffer.ReadByte();
        }

        /// <inheritdoc />
        public int GetLengthInBits(byte value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(byte a, byte b)
        {
            return a.Equals(b);
        }
        
        #endregion
    }
}