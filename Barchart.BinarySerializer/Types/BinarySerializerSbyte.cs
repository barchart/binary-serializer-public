using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerSbyte : IBinaryTypeSerializer<sbyte>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(sbyte) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, sbyte value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteByte((byte)value);
        }

        /// <inheritdoc />
        public Attribute<sbyte> Decode(IDataBuffer dataBuffer)
        {
            return new Attribute<sbyte>(Header.ReadFromBuffer(dataBuffer), (sbyte)dataBuffer.ReadByte());
        }

        /// <inheritdoc />
        public int GetLengthInBits(sbyte value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}