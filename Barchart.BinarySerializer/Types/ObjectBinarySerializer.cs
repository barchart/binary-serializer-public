using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for objects of type <typeparamref name="TContainer"/> using a specified schema.
    /// </summary>
    /// <typeparam name="TContainer">The type of objects to be serialized.</typeparam>
    public class ObjectBinarySerializer<TContainer> : IBinaryTypeObjectSerializer<TContainer> where TContainer : new()
    {
        public Schema<TContainer> Schema { get; set; }

        public ObjectBinarySerializer(Schema<TContainer> schema)
        {
            Schema = schema;
        }

        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = Schema.Deserialize(dataBuffer);

            return new HeaderWithValue<TContainer>
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer, TContainer existing)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new HeaderWithValue<TContainer>
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        public void Encode(DataBuffer dataBuffer, TContainer? oldObject, TContainer? newObject)
        {
            Header header = new() { IsMissing = false, IsNull = newObject == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            Schema.Serialize(oldObject!, newObject!, dataBuffer);
        }

        public int GetLengthInBytes(TContainer value)
        {
            return Schema.GetLengthInBytes(value);
        }

        public int GetLengthInBits(TContainer? value)
        {
            return ((ISchema)Schema).GetLengthInBits(value);
        }

        public int GetLengthInBits(TContainer? oldValue, TContainer? newValue)
        {
            return ((ISchema)Schema).GetLengthInBits(oldValue, newValue);
        }
    }
}