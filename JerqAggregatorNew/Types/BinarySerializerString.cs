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
                    buffer.WriteBit((byte)((valueLength >> i) & 1), ref offset, ref offsetInLastByte);
                }

                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    buffer.WriteByte(valueBytes[i], ref offset, ref offsetInLastByte);
                }
            }
        }

        public HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            byte[]? valueBytes = null;
            int size = 0;

            Header header = new Header();

            header.IsMissing = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            for (int i = 5; i >= 0; i--)
            {
                byte bit = buffer.ReadBit(ref offset, ref offsetInLastByte);
                size |= (bit << i);
            }

            valueBytes = new byte[size];

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = buffer.ReadByte(ref offset, ref offsetInLastByte);
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