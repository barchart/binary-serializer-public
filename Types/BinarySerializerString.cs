﻿using Barchart.BinarySerializer.Schemas;
using System.Text;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerString : IBinaryTypeSerializer<string>
    {
        public const int NUMBER_OF_HEADER_BITS_NULL_STRING = 2;
        public const int NUMBER_OF_HEADER_BITS_STRING = 8;

        public void Encode(DataBuffer dataBuffer, string? value)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

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

        public HeaderWithValue Decode(DataBuffer dataBuffer)
        {
            byte[]? valueBytes = null;
            int size = 0;

            Header header = new Header();

            header.IsMissing = dataBuffer.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            for (int i = 5; i >= 0; i--)
            {
                byte bit = dataBuffer.ReadBit();
                size |= (bit << i);
            }

            valueBytes = new byte[size];

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            string decodedString = Encoding.UTF8.GetString(valueBytes);
            return new HeaderWithValue(header, decodedString);
        }

        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_NULL_STRING;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + NUMBER_OF_HEADER_BITS_STRING;
        }

        #region ISerializer implementation
        void ISerializer.Encode(DataBuffer dataBuffer, object? value)
        {
            Encode(dataBuffer, (string?)value);
        }
        HeaderWithValue ISerializer.Decode(DataBuffer dataBuffer)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<string>)this).Decode(dataBuffer);
        }

        int ISerializer.GetLengthInBits(object? value)
        {
            return GetLengthInBits((string?)value);
        }
        #endregion
    }
}