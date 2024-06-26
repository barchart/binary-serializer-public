#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) bytes to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerByte : IBinaryTypeSerializer<byte>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(byte) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, byte value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteByte(value);
        }

        /// <inheritdoc />
        public Attribute<byte> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            byte decodedValue = default;
            
            if (!valueIsMissing && !valueIsNull)
            {
                decodedValue = dataBuffer.ReadByte();
            }

            return new Attribute<byte>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(byte value)
        {
            return ENCODED_LENGTH_BITS;
        }
        
        #endregion
    }
}