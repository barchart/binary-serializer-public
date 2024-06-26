using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUShort : IBinaryTypeSerializer<ushort>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(ushort) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, ushort value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<char> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(ushort));
            ushort decodedValue = BitConverter.ToUInt16(valueBytes);
                
            return new Attribute<ushort>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(ushort value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}