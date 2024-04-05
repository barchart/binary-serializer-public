using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a base class for binary serializers handling nullable numeric types.
    /// </summary>
    /// <typeparam name="T">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNullableNumeric<T> : IBinaryTypeSerializer<T?> where T : struct
    {
        public const int NumberOfHeaderBitsNumeric = 2;

        public abstract int Size { get; }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            Header header = new()
            {
                IsMissing = false,
                IsNull = value == null
            };

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value.HasValue)
            {
                byte[] valueBytes = ConvertToByteArray(value.Value);

                for (int i = valueBytes.Length - 1; i >= 0; i--)
                {
                    dataBuffer.WriteByte(valueBytes[i]);
                }
            }
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer)
        {
            int size = Size;
            byte[] valueBytes = new byte[size];

            Header header = new()
            {
                IsMissing = dataBuffer.ReadBit() == 1
            };

            if (header.IsMissing)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            return new HeaderWithValue<T?>(header, DecodeBytes(valueBytes));
        }

        public int GetLengthInBits(T? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNumeric;
            }

            return Size * 8 + NumberOfHeaderBitsNumeric;
        }

        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes);
    }
}