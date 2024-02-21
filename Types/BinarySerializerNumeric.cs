using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    public abstract class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T?> where T : struct
    {
        public abstract int Size { get; }

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
                    buffer.WriteByte(valueBytes[i], ref offset, ref offsetInLastByte);
                }
            }
        }

        public HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            int size = Size;
            byte[] valueBytes = new byte[size];

            Header header = new Header();
            header.IsMissing = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = buffer.ReadByte(ref offset, ref offsetInLastByte);
            }

            return new HeaderWithValue(header, DecodeBytes(valueBytes, offset));
        }
        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes, int offset);
        public abstract int GetLengthInBytes(T? value);

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