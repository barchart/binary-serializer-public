using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerEnum<T> : IBinaryTypeObjectSerializer<T?> where T : struct, Enum
    {
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<int> _serializer;

        public BinarySerializerEnum(IBinaryTypeSerializer<int> serializer)
        {
            _serializer = serializer;
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer, T? existing)
        {
            Header header = ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            int enumValue = _serializer.Decode(dataBuffer).Value;
            T value = (T)Enum.ToObject(typeof(T), enumValue);

            return new HeaderWithValue<T?>(header, value);
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer)
        {
            return Decode(dataBuffer, null);
        }

        public void Encode(DataBuffer dataBuffer, T? oldValue, T? newValue)
        {
            Encode(dataBuffer, newValue);
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            WriteHeader(dataBuffer, header);

            if (value != null)
            {
                _serializer.Encode(dataBuffer, Convert.ToInt32(value));
            }
        }

        public int GetLengthInBits(T? value)
        {
            return value != null ? _serializer.GetLengthInBits(Convert.ToInt32(value)) + NumberOfHeaderBitsNumeric : NumberOfHeaderBitsNumeric;
        }

        private static Header ReadHeader(DataBuffer dataBuffer)
        {
            return BufferHelper.ReadHeader(dataBuffer);
        }

        private static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            BufferHelper.WriteHeader(dataBuffer, header);
        }
    }
}
