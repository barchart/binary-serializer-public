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
            Header header = new() { IsMissing = false, IsNull = value == null };
            WriteHeader(dataBuffer, header);

            if (value.HasValue)
            {
                byte[] valueBytes = ConvertToByteArray(value.Value);
                WriteValueBytes(dataBuffer, valueBytes);
            }
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            byte[] valueBytes = ReadValueBytes(dataBuffer);

            return new HeaderWithValue<T?>(header, DecodeBytes(valueBytes));
        }

        public int GetLengthInBits(T? value)
        {
            return value == null ? NumberOfHeaderBitsNumeric : Size * 8 + NumberOfHeaderBitsNumeric;
        }

        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes);

        private static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
        }

        private static Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private static void WriteValueBytes(DataBuffer dataBuffer, byte[] valueBytes)
        {
            for (int i = valueBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }

        private byte[] ReadValueBytes(DataBuffer dataBuffer)
        {
            byte[] valueBytes = new byte[Size];
            for (int i = Size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }
            return valueBytes;
        }
    }
}