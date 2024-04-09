using Barchart.BinarySerializer.Schemas;
using System.Text;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for string members of the class or structure.
    /// </summary>
    public class BinarySerializerString : IBinaryTypeSerializer<string?>
    {
        public const int NumberOfHeaderBitsNullString = 2;
        public const int NumberOfHeaderBitsString = 8;

        public void Encode(DataBuffer dataBuffer, string? value)
        {
            Header header = new()
            {
                IsMissing = false,
                IsNull = value == null
            };

            WriteHeader(dataBuffer, header);

            if (value != null)
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                WriteStringLength(dataBuffer, valueBytes.Length);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    dataBuffer.WriteByte(valueBytes[i]);
                }
            }
        }

        public HeaderWithValue<string?> Decode(DataBuffer dataBuffer)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<string?>(header, default);
            }

            int size = ReadStringLength(dataBuffer);
            byte[]? valueBytes = new byte[size];

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            string decodedString = Encoding.UTF8.GetString(valueBytes);
            return new HeaderWithValue<string?>(header, decodedString);
        }

        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNullString;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + NumberOfHeaderBitsString;
        }

        private void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
        }

        private Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private void WriteStringLength(DataBuffer dataBuffer, int length)
        {
            for (int i = 5; i >= 0; i--)
            {
                dataBuffer.WriteBit((byte)((length >> i) & 1));
            }
        }

        private int ReadStringLength(DataBuffer dataBuffer)
        {
            int size = 0;

            for (int i = 5; i >= 0; i--)
            {
                byte bit = dataBuffer.ReadBit();
                size |= (bit << i);
            }

            return size;
        }
    }
}