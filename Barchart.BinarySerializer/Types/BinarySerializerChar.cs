#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) characters to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerChar : IBinaryTypeSerializer<char>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(char) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, char value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<char> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(char));
            char decodedValue = BitConverter.ToChar(valueBytes);
                
            return new Attribute<char>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(char value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}