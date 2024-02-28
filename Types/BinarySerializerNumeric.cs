using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    public abstract class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T?> where T : struct
    {
        public const int NUMBER_OF_HEADER_BITS_NUMERIC = 2;

        public abstract int Size { get; }

        public void Encode(BufferHelper bufferHelper, T? value)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            bufferHelper.WriteBit(0);
            bufferHelper.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value.HasValue)
            {
                byte[] valueBytes = ConvertToByteArray(value.Value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    bufferHelper.WriteByte(valueBytes[i]);
                }
            }
        }

        public HeaderWithValue Decode(BufferHelper bufferHelper)
        {
            int size = Size;
            byte[] valueBytes = new byte[size];

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

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = bufferHelper.ReadByte();
            }

            return new HeaderWithValue(header, DecodeBytes(valueBytes));
        }

        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes);
        public abstract int GetLengthInBits(T? value);

        #region ISerializer implementation
        void ISerializer.Encode(BufferHelper bufferHelper, object? value)
        {
            Encode(bufferHelper, (T?)value);
        }
        HeaderWithValue ISerializer.Decode(BufferHelper bufferHelper)
        {
            return ((IBinaryTypeSerializer<T?>)this).Decode(bufferHelper);
        }

        int ISerializer.GetLengthInBits(object? value)
        {
            return GetLengthInBits((T?)value);
        }
        #endregion
    }
}