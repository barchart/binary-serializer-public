using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides binary serialization functionality for nullable value types.
    /// This class implements the IBinaryTypeSerializer interface for nullable value types.
    /// </summary>
    /// <typeparam name="TMember">The underlying value type of the nullable type.</typeparam>
    public class BinarySerializerNullable<TMember> : IBinaryTypeSerializer<TMember?> where TMember : struct
	{
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
                Utility.UtilityKit.WriteHeader(dataBuffer, header);
            }
        }

        public HeaderWithValue<TMember?> Decode(DataBuffer dataBuffer)
        {
            HeaderWithValue<TMember> headerWithValue = _serializer.Decode(dataBuffer);

            if (headerWithValue.Header.IsNull || headerWithValue.Header.IsMissing) 
            {
                return new HeaderWithValue<TMember?>(headerWithValue.Header, null);
            }

            return new HeaderWithValue<TMember?>(headerWithValue.Header, headerWithValue.Value);
        }

        public int GetLengthInBits(TMember? value)
        {
            return value == null ? Utility.UtilityKit.NumberOfHeaderBitsNonString : _serializer.GetLengthInBits((TMember)value);
        }
    }
}