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
        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, string? value)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = value == null });

            if (value != null)
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                if (valueBytes.Length > 0x3F)
                {
                    throw new InvalidOperationException("String length exceeds 6-bit encoding capacity.");
                }

                int lengthIn6Bits = valueBytes.Length & 0x3F; 

                for (int i = 5; i >= 0; i--)
                {
                    dataBuffer.WriteBit((lengthIn6Bits >> i) & 1);
                }

                dataBuffer.WriteBytes(valueBytes);
            }
        }

        /// <inheritdoc />
        public Attribute<string?> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<string?>(header, default);
            }
            
            int size = 0;
            
            for (int i = 5; i >= 0; i--)
            {
                size |= (dataBuffer.ReadBit() ? 1 : 0) << i;
            }

            byte[] valueBytes = dataBuffer.ReadBytes(size);
            string decodedString = Encoding.UTF8.GetString(valueBytes);

            return new Attribute<string?>(header, decodedString);
        }

        /// <inheritdoc />
        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return Header.NumberOfHeaderBitsNonString;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + Header.NumberOfHeaderBitsString;
        }

        #endregion
    }
}