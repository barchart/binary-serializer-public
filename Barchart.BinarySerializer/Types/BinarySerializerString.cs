#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;
using System.Text;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for string members of the class or structure.
    /// </summary>
    public class BinarySerializerString : IBinaryTypeSerializer<string?>
    {
        #region Methods

        public void Encode(DataBuffer dataBuffer, string? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };

            header.WriteToBuffer(dataBuffer);

            if (value != null)
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                UtilityKit.WriteLength(dataBuffer, valueBytes.Length);
                UtilityKit.WriteValueBytes(dataBuffer, valueBytes);
            }
        }

        public HeaderWithValue<string?> Decode(DataBuffer dataBuffer)
        {
            Header header = UtilityKit.ReadHeader(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<string?>(header, default);
            }

            int size = UtilityKit.ReadLength(dataBuffer);

            byte[] valueBytes = UtilityKit.ReadValueBytes(dataBuffer, size);
            string decodedString = Encoding.UTF8.GetString(valueBytes);

            return new HeaderWithValue<string?>(header, decodedString);
        }

        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return UtilityKit.NumberOfHeaderBitsNonString;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + UtilityKit.NumberOfHeaderBitsString;
        }

        #endregion
    }
}