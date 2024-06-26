#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerULong : IBinaryTypeSerializer<ulong>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = sizeof(ulong) * 8;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, ulong value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<ulong> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            ulong decodedValue = default;

            if (!valueIsMissing && !valueIsNull)
            {
                byte[] valueBytes = dataBuffer.ReadBytes(sizeof(ulong));
                decodedValue = BitConverter.ToUInt64(valueBytes);
            }
                
            return new Attribute<ulong>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(ulong value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}