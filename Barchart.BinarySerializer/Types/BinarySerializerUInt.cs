using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerUInt : IBinaryTypeSerializer<uint>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(uint) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, uint value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<char> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(uint));
            uint decodedValue = BitConverter.ToUInt32(valueBytes);
                
            return new Attribute<uint>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(uint value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}