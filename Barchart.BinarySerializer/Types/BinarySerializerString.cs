#region Using Statements

using Barchart.BinarySerializer.SerializationUtilities.Headers;
using Barchart.BinarySerializer.Schemas;
using System.Text;

#endregion

namespace Barchart.BinarySerializer.SerializationUtilities.Types
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
            Header header = new() { IsMissing = false, IsNull = value == null };

            header.WriteToBuffer(dataBuffer);

            if (value != null)
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                dataBuffer.WriteLength(valueBytes.Length);
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

            int size = dataBuffer.ReadLength();

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