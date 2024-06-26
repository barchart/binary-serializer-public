#region Using Statements

using System.Text;
using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for string types.
    /// </summary>
    public class BinarySerializerString : IBinaryTypeSerializer<string?>
    {
        #region Constants

        private const int MAX_STRING_LENGTH = 0x3F;

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBufferWriter dataBuffer, string? value)
        {
            if (value == null)
            {
                Header.WriteToBuffer(dataBuffer, false, true);
            }
            else 
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                if (valueBytes.Length > MAX_STRING_LENGTH)
                {
                    throw new InvalidOperationException("String length exceeds 6-bit encoding capacity.");
                }

                int lengthIn6Bits = valueBytes.Length & 0x3F; 
                Header.WriteToBuffer(dataBuffer, false, false, lengthIn6Bits);

                dataBuffer.WriteBytes(valueBytes);
            }
        }

        /// <inheritdoc />
        public Attribute<string?> Decode(IDataBufferReader dataBuffer)
        {
            Header.ReadFromBuffer(dataBuffer, out bool valueIsMissing, out bool valueIsNull);
            string? decodedString = default;
            
            if (!valueIsMissing && !valueIsNull)
            {
                 int size = 0;
            
                for (int i = 5; i >= 0; i--)
                {
                    size |= (dataBuffer.ReadBit() ? 1 : 0) << i;
                }

                byte[] valueBytes = dataBuffer.ReadBytes(size);
                decodedString = Encoding.UTF8.GetString(valueBytes);
            }
            
            return new Attribute<string?>(valueIsMissing, decodedString);
        }

        /// <inheritdoc />
        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return Header.NUMBER_OF_HEADER_BITS_NON_STRING;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + Header.NUMBER_OF_HEADER_BITS_STRING;
        }

        #endregion
    }
}