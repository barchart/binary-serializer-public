#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

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
        public void Encode(IDataBufferWriter dataBuffer, ushort value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBytes(BitConverter.GetBytes(value));
        }

        /// <inheritdoc />
        public Attribute<ushort> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            byte[] valueBytes = dataBuffer.ReadBytes(sizeof(ushort));
            ushort decodedValue = BitConverter.ToUInt16(valueBytes);
                
            return new Attribute<ushort>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(ushort value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}