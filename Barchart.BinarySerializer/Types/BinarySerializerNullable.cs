using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
	public class BinarySerializerNullable<TMember> : IBinaryTypeSerializer<TMember?> where TMember : struct
	{
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<TMember> _serializer;

        public BinarySerializerNullable(IBinaryTypeSerializer<TMember> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, TMember? value)
        {
            if (value != null)
            {
                _serializer.Encode(dataBuffer, (TMember)value);
            }
            else
            {
                Header header = new() { IsMissing = false, IsNull = true };
                BufferHelper.WriteHeader(dataBuffer , header);
            }
        }

        public HeaderWithValue<TMember?> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<TMember> headerWithValue = _serializer.Decode(dataBuffer);

            if(headerWithValue.Header.IsNull || headerWithValue.Header.IsMissing) 
            {
                return new HeaderWithValue<TMember?>(headerWithValue.Header, null);
            }

            return new HeaderWithValue<TMember?>(headerWithValue.Header, headerWithValue.Value);
        }

        public int GetLengthInBits(TMember? value)
        {
            return value == null ? NumberOfHeaderBitsNumeric : _serializer.GetLengthInBits((TMember)value);
        }
    }
}