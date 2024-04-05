﻿using Barchart.BinarySerializer.Schemas;
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

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value != null)
            {
                // writing size of string in the buffer
                byte valueLength = (byte)Encoding.UTF8.GetByteCount(value);
                header.StringLength = valueLength;

                for (int i = 5; i >= 0; i--)
                {
                    dataBuffer.WriteBit((byte)((valueLength >> i) & 1));
                }

                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    dataBuffer.WriteByte(valueBytes[i]);
                }
            }
        }

        public HeaderWithValue<string?> Decode(DataBuffer dataBuffer)
        {
            int size = 0;

            Header header = new()
            {
                IsMissing = dataBuffer.ReadBit() == 1
            };

            if (header.IsMissing)
            {
                return new HeaderWithValue<string?>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue<string?>(header, default);
            }

            for (int i = 5; i >= 0; i--)
            {
                byte bit = dataBuffer.ReadBit();
                size |= (bit << i);
            }

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
    }
}