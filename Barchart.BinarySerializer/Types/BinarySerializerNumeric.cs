using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a base class for binary serializers handling numeric types.
    /// </summary>
    /// <typeparam name="T">The underlying numeric type.</typeparam>
    public abstract class BinarySerializerNumeric<T> : IBinaryTypeSerializer<T> where T : struct
    {
        public const int NUMBER_OF_HEADER_BITS_NUMERIC = 2;

        public abstract int Size { get; }

        public void Encode(DataBuffer dataBuffer, T value)
        {
            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit(0);
     
            byte[] valueBytes = ConvertToByteArray(value);

            for (int i = valueBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }

        public HeaderWithValue<T> Decode(DataBuffer dataBuffer)
        {
            int size = Size;
            byte[] valueBytes = new byte[size];

            Header header = new()
            {
                IsMissing = dataBuffer.ReadBit() == 1
            };

            if (header.IsMissing)
            {
                return new HeaderWithValue<T>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            for (int i = size - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            return new HeaderWithValue<T>(header, DecodeBytes(valueBytes));
        }

        public int GetLengthInBits(T value)
        {
            return Size * 8 + NUMBER_OF_HEADER_BITS_NUMERIC;
        }

        protected abstract byte[] ConvertToByteArray(T value);
        protected abstract T DecodeBytes(byte[] bytes);
    }
}