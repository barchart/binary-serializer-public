using JerqAggregatorNew.Schemas;
using System.Runtime.CompilerServices;

namespace JerqAggregatorNew.Types
{
    public abstract class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T?> where T : struct, IConvertible
    {
        protected abstract int Size { get; }
        public void Encode(byte[] buffer, T? value, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            buffer.WriteBit(0, ref offset, ref offsetInLastByte);
            buffer.WriteBit((byte)(header.IsNull ? 1 : 0), ref offset, ref offsetInLastByte);

            if (value.HasValue)
            {
                byte[] valueBytes = ConvertToByteArray(value.Value);

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
            int size = Size;
            byte[] valueBytes = new byte[size];

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

            return new HeaderWithValue(header, DecodeBytes(valueBytes, offset));         
        }
        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes, int offset);
        protected abstract int GetLengthInBytes(T? value);

        #region ISerializer implementation
        void ISerializer.Encode(byte[] buffer, object? value, ref int offset, ref int offsetInLastByte)
        {
            Encode(buffer, (T?)value, ref offset, ref offsetInLastByte);
        }
        HeaderWithValue ISerializer.Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            return (HeaderWithValue)((IBinaryTypeSerializer<T?>)this).Decode(buffer, ref offset, ref offsetInLastByte);
        }

        int ISerializer.GetLengthInBytes(object? value)
        {
            return GetLengthInBytes((T?)value);
        }
        #endregion
    }
}
