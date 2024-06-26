using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerInt : IBinaryTypeSerializer<int>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(int) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, int value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<int> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(int));
            int decodedValue = BitConverter.ToInt32(valueBytes);

            return new Attribute<int>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(int value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}