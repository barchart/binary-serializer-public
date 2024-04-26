using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides binary serialization functionality for enum types.
    /// This class implements the IBinaryTypeSerializer interface for enum types.
    /// </summary>
    /// <typeparam name="TMember">The enum type to be serialized.</typeparam>
    public class BinarySerializerEnum<TMember> : IBinaryTypeSerializer<TMember> where TMember : Enum
    {
        private readonly BinarySerializerInt32 _serializer;

        public BinarySerializerEnum(BinarySerializerInt32 serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, TMember? value)
        {
            int? val = value != null? Convert.ToInt32(value) : null;

            if (val != null)
            {
                _serializer.Encode(dataBuffer, (int)val);
            }
        }

        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<int> headerWithValue = _serializer.Decode(dataBuffer);
            int value = headerWithValue.Value;

            return new HeaderWithValue<TMember>(headerWithValue.Header, (TMember?)Enum.Parse(typeof(TMember), value.ToString(), true));
        }

        public int GetLengthInBits(TMember? value)
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