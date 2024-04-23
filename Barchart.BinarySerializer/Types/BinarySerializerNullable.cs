using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
	public class BinarySerializerNullable<TContainer> : IBinaryTypeSerializer<TContainer?> where TContainer : struct
	{
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<TContainer> _serializer;

        public BinarySerializerNullable(IBinaryTypeSerializer<TContainer> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (TContainer)value);
            }
            else
            {
                Header header = new() { IsMissing = false, IsNull = true };
                BufferHelper.WriteHeader(dataBuffer , header);
            }
        }

        public HeaderWithValue<TContainer?> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<TContainer> headerWithValue = _serializer.Decode(dataBuffer);

            if(headerWithValue.Header.IsNull || headerWithValue.Header.IsMissing) 
            {
                return new HeaderWithValue<TContainer?>(headerWithValue.Header, null);
            }

            return new HeaderWithValue<TContainer?>(headerWithValue.Header, headerWithValue.Value);
        }

        public int GetLengthInBits(TContainer? value)
        {
            return value == null ? NumberOfHeaderBitsNumeric : _serializer.GetLengthInBits((TContainer)value);
        }
    }
}