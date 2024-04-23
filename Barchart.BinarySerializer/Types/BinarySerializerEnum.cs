using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerEnum<TContainer> : IBinaryTypeSerializer<TContainer> where TContainer : Enum
    {
        private readonly BinarySerializerInt32 _serializer;

        public BinarySerializerEnum(BinarySerializerInt32 serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            int? val = value != null? Convert.ToInt32(value) : null;

            if (val != null)
            {
                _serializer.Encode(dataBuffer, (int)val);
            }
        }

        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<int> headerWithValue = _serializer.Decode(dataBuffer);
            int value = headerWithValue.Value;

            return new HeaderWithValue<TContainer>(headerWithValue.Header, (TContainer?)Enum.Parse(typeof(TContainer), value.ToString(), true));
        }

        public int GetLengthInBits(TContainer? value)
        {
            int? val = value != null ? Convert.ToInt32(value) : null;

            if (val != null)
            {
                return _serializer.GetLengthInBits((int)val);

            }
            return 0;
        }
    }
}