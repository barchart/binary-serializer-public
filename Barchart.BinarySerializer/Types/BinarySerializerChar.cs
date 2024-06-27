#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Common;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) characters to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerChar : IBinaryTypeSerializer<char>
    {
        #region Constants

        private const int ENCODED_LENGTH_IN_BYTES = sizeof(char);
        private const int ENCODED_LENGTH_IN_BITS = ENCODED_LENGTH_IN_BYTES * Constants.BITS_PER_BYTE;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter buffer, char value) 
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public char Decode(IDataBufferReader buffer)
        {
            return BitConverter.ToChar(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
        }

        /// <inheritdoc />
        public int GetLengthInBits(char value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }
        
        /// <inheritdoc />
        public bool GetEquals(char a, char b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}