#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) boolean values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerBool : IBinaryTypeSerializer<bool>
    {
        #region Constants
        
        private const int ENCODED_LENGTH_IN_BITS = 1;
        
        #endregion
        
        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, bool value)
        {
            dataBuffer.WriteBit(value);
        }

        /// <inheritdoc />
        public bool Decode(IDataBufferReader dataBuffer)
        {
            return dataBuffer.ReadBit();
        }

        /// <inheritdoc />
        public int GetLengthInBits(bool value)
        {
            return ENCODED_LENGTH_IN_BITS;
        }

        /// <inheritdoc />
        public bool GetEquals(bool a, bool b)
        {
            return a.Equals(b);
        }

        #endregion
    }
}