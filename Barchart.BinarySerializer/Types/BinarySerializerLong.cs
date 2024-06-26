using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerLong: IBinaryTypeSerializer<long>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(long) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, long value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<float> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(long));
            float decodedValue = BitConverter.ToInt64(valueBytes);
            return new Attribute<long>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(long value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}