using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerEnum<T> : IBinaryTypeSerializer<T> where T : Enum
    {
        private readonly BinarySerializerInt32 _serializer;


        public BinarySerializerEnum(BinarySerializerInt32 serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            int? val = value != null? Convert.ToInt32(value) : null;
            _serializer.Encode(dataBuffer, (int)val);
        }

        public HeaderWithValue<T> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<int> headerWithValue = _serializer.Decode(dataBuffer);
            int value = headerWithValue.Value;

            return new HeaderWithValue<T>(headerWithValue.Header, (T?)Enum.Parse(typeof(T), value.ToString(), true));
        }

        public int GetLengthInBits(T? value)
        {
            int? val = value != null ? Convert.ToInt32(value) : null;
            return _serializer.GetLengthInBits((int)val);
        }
    }
}
