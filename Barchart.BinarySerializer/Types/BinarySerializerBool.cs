#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Reads (and writes) boolean values to (and from) a binary data source.
    /// </summary>
    public class BinarySerializerBool : IBinaryTypeSerializer<bool>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH_BITS = 2;
        private const int ENCODED_VALUE_LENGTH_BITS = 1;
        
        private const int ENCODED_LENGTH_BITS = ENCODED_HEADER_LENGTH_BITS + ENCODED_VALUE_LENGTH_BITS;
        
        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, bool value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBit(value);
        }

        /// <inheritdoc />
        public Attribute<bool> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            bool decodedValue = default;

            if (!valueIsMissing && !valueIsNull)
            {
                decodedValue = dataBuffer.ReadBit();
            }

            return new Attribute<bool>(valueIsMissing, decodedValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(bool value)
        {
            return ENCODED_LENGTH_BITS;
        }

        #endregion
    }
}