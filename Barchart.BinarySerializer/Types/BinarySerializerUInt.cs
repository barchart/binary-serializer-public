#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

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
        public void Encode(IDataBufferWriter dataBuffer, uint value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<uint> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            uint decodedValue = default;

            if (!valueIsMissing && !valueIsNull)
            {
                byte[] valueBytes = dataBuffer.ReadBytes(sizeof(uint));
                decodedValue = BitConverter.ToUInt32(valueBytes);
            }

            return new Attribute<uint>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(uint value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}