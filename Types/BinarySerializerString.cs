﻿using JerqAggregatorNew.Schemas;
using System.Text;

namespace JerqAggregatorNew.Types
{
    public class BinarySerializerString : IBinaryTypeSerializer<string>
    {
        public void Encode(BufferHelper bufferHelper, string? value)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            bufferHelper.WriteBit(0);
            bufferHelper.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value != null)
            {
                // writing size of string in the buffer
                byte valueLength = (byte)Encoding.UTF8.GetByteCount(value);
                header.StringLength = valueLength;

                for (int i = 5; i >= 0; i--)
                {
                    bufferHelper.WriteBit((byte)((valueLength >> i) & 1));
                }

                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    bufferHelper.WriteByte(valueBytes[i]);
                }
            }
        }

        public HeaderWithValue Decode(BufferHelper bufferHelper)
        {
            byte[]? valueBytes = null;
            int size = 0;

            Header header = new Header();

            header.IsMissing = bufferHelper.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = bufferHelper.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            for (int i = 5; i >= 0; i--)
            {
                byte bit = bufferHelper.ReadBit();
                size |= (bit << i);
            }

            valueBytes = new byte[size];

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = bufferHelper.ReadByte();
            }

            string decodedString = Encoding.UTF8.GetString(valueBytes);
            return new HeaderWithValue(header, decodedString);
        }

        public int GetLengthInBytes(string? value)
        {
            if (value == null)
            {
                return sizeof(byte);
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength + sizeof(byte);
        }

        #region ISerializer implementation
        void ISerializer.Encode(BufferHelper bufferHelper, object? value)
        {
            Encode(bufferHelper, (string?)value);
        }
        HeaderWithValue ISerializer.Decode(BufferHelper bufferHelper)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<string>)this).Decode(bufferHelper);
        }

        int ISerializer.GetLengthInBytes(object? value)
        {
            return GetLengthInBytes((string?)value);
        }
        #endregion
    }
}