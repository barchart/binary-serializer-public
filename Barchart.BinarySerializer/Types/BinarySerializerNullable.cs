using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
	public class BinarySerializerNullable<T> : IBinaryTypeSerializer<T?> where T : struct
	{
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<T> _serializer;

        public BinarySerializerNullable(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (T)value);
            }
            else
            {
                Header header = new() { IsMissing = false, IsNull = true };
                BufferHelper.WriteHeader(dataBuffer , header);
            }
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<T> headerWithValue = _serializer.Decode(dataBuffer);

            if(headerWithValue.Header.IsNull || headerWithValue.Header.IsMissing) 
            {
                return new HeaderWithValue<T?>(headerWithValue.Header, null);
            }

            return new HeaderWithValue<T?>(headerWithValue.Header, headerWithValue.Value);
        }

        public int GetLengthInBits(T? value)
        {
            return value == null ? NumberOfHeaderBitsNumeric : _serializer.GetLengthInBits((T)value);
        }
    }
}