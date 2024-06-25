#region Using Statements

using Barchart.BinarySerializer.Attributes;
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
        
        private const int ENCODED_HEADER_LENGTH = 2;
        private const int ENCODED_VALUE_LENGTH = 1;
        
        private const int ENCODED_LENGTH = ENCODED_HEADER_LENGTH + ENCODED_VALUE_LENGTH;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, bool value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBit(value);
        }

        /// <inheritdoc />
        public Attribute<bool> Decode(IDataBuffer dataBuffer)
        {
            return new Attribute<bool>(Header.ReadFromBuffer(dataBuffer), dataBuffer.ReadBit());
        }

        /// <inheritdoc />
        public int GetLengthInBits(bool value)
        {
            return ENCODED_LENGTH;
        }

        #endregion
    }
}