#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

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
        public void Encode(IDataBufferWriter dataBuffer, long value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<long> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);

            if (valueIsMissing || valueIsNull)
            {
                return new Attribute<long>(valueIsMissing, default);
            }

            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(long));
            long decodedValue = BitConverter.ToInt64(valueBytes);

            return new Attribute<long>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(long value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}