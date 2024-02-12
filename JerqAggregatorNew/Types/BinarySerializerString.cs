using JerqAggregatorNew.Schemas;
using System.Text;

namespace JerqAggregatorNew.Types
{
    public class BinarySerializerString : IBinaryTypeSerializer<string>
    {
        public void Encode(byte[] buffer, string? value, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            buffer.WriteBit(0, ref offset, ref offsetInLastByte);
            buffer.WriteBit((byte)(header.IsNull ? 1 : 0), ref offset, ref offsetInLastByte);

            if (value != null)
            {
                // writing size of string in the buffer
                byte valueLength = (byte)Encoding.UTF8.GetByteCount(value);
                header.StringLength = valueLength;

                for (int i = 5; i >= 0; i--)
                {
                    if (offsetInLastByte % 8 == 0)
                    {
                        offset++;
                        buffer[offset] = 0;
                        offsetInLastByte = 0;
                    }

                    byte bit = (byte)((valueLength >> i) & 1);
                    buffer[offset] |= (byte)(bit << (7 - offsetInLastByte));
                    offsetInLastByte++;

                }

                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        if (offsetInLastByte % 8 == 0)
                        {
                            offset++;
                            buffer[offset] = 0;
                            offsetInLastByte = 0;
                        }

                        buffer[offset] |= (byte)(((valueBytes[i] >> j) & 1) << ((7 - offsetInLastByte) % 8));
                        offsetInLastByte++;
                    }
                }
            }
        }

        public HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            byte[]? valueBytes = null;
            int size = 0;

            Header header = new Header();

            if (offsetInLastByte == 8)
            {
                offsetInLastByte = 0;
                offset++;
            }

            header.IsMissing = ((buffer[offset] >> (7 - offsetInLastByte)) & 1) == 1;

            offsetInLastByte++;

            if (offsetInLastByte == 8)
            {
                offsetInLastByte = 0;
                offset++;
            }

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = ((buffer[offset] >> (7 - offsetInLastByte)) & 1) == 1;

            offsetInLastByte++;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            for (int i = 5; i >= 0; i--)
            {
                if (offsetInLastByte % 8 == 0)
                {
                    offset++;
                    offsetInLastByte = 0;
                }

                int bit = (buffer[offset] >> (7 - offsetInLastByte)) & 1;
                size |= (bit << i);

                offsetInLastByte++;
            }

            valueBytes = new byte[size];

            for (int i = size - 1; i >= 0; i--)
            {
                byte byteToAdd = 0;
                for (int j = 7; j >= 0; j--)
                {
                    if (offsetInLastByte % 8 == 0)
                    {
                        offset++;
                        offsetInLastByte = 0;
                    }

                    int bit = (buffer[offset] >> (7 - offsetInLastByte)) & 1;
                    byteToAdd |= (byte)(bit << j);
                    offsetInLastByte++;
                    
                }
                valueBytes[i] = byteToAdd;
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
        void ISerializer.Encode(byte[] buffer, object? value, ref int offset, ref int offsetInLastByte)
        {
            Encode(buffer, (string?)value, ref offset, ref offsetInLastByte);
        }
        HeaderWithValue ISerializer.Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<string>)this).Decode(buffer, ref offset, ref offsetInLastByte);
        }

        int ISerializer.GetLengthInBytes(object? value)
        {
            return GetLengthInBytes((string?)value);
        }
        #endregion
    }
}
