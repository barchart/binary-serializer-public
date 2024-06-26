#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) float values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerFloat : IBinaryTypeSerializer<float>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(float) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, float value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<float> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(float));
            float decodedValue = BitConverter.ToSingle(valueBytes);
            return new Attribute<float>(header, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(float value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}