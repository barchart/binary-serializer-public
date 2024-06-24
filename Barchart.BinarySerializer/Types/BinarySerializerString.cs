#region Using Statements

using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Schemas;
using System.Text;

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
        public void Encode(DataBuffer dataBuffer, string? value)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = value == null });

            if (value != null)
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                dataBuffer.WriteBytes(BitConverter.GetBytes(valueBytes.Length));
                dataBuffer.WriteBytes(valueBytes);
            }
        }

        /// <inheritdoc />
        public HeaderWithValue<string?> Decode(DataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<string?>(header, default);
            }
            
            int size = BitConverter.ToInt32(dataBuffer.ReadBytes(sizeof(int)));

            byte[] valueBytes = dataBuffer.ReadBytes(size);
            string decodedString = Encoding.UTF8.GetString(valueBytes);

            return new HeaderWithValue<string?>(header, decodedString);
        }

        /// <inheritdoc />
        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return DataBuffer.NumberOfHeaderBitsNonString;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + DataBuffer.NumberOfHeaderBitsString;
        }

        #endregion
    }
}