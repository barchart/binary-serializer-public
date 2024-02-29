using Barchart.BinarySerializer.Schemas;
using System.Text;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerString : IBinaryTypeSerializer<string>
    {
        public const int NUMBER_OF_HEADER_BITS_STRING = 8;

        public void Encode(DataBuffer bufferHelper, string? value)
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

        public HeaderWithValue Decode(DataBuffer bufferHelper)
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

        public int GetLengthInBits(string? value)
        {
            if (value == null)
            {
                return NUMBER_OF_HEADER_BITS_STRING;
            }

            int valueLength = Encoding.UTF8.GetByteCount(value);
            return valueLength * 8 + NUMBER_OF_HEADER_BITS_STRING;
        }

        #region ISerializer implementation
        void ISerializer.Encode(DataBuffer bufferHelper, object? value)
        {
            Encode(bufferHelper, (string?)value);
        }
        HeaderWithValue ISerializer.Decode(DataBuffer bufferHelper)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<string>)this).Decode(bufferHelper);
        }

        int ISerializer.GetLengthInBits(object? value)
        {
            return GetLengthInBits((string?)value);
        }
        #endregion
    }
}