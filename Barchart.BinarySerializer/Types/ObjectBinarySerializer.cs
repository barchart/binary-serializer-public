using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for objects of type <typeparamref name="T"/> using a specified schema.
    /// </summary>
    /// <typeparam name="T">The type of objects to be serialized.</typeparam>
    public class ObjectBinarySerializer<T> : IBinaryTypeObjectSerializer<T> where T : new()
    {
        public Schema<T> Schema { get; set; }

        public ObjectBinarySerializer(Schema<T> schema)
        {
            Schema = schema;
        }

        public HeaderWithValue<T> Decode(DataBuffer dataBuffer)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T>(header, default);
            }

            T? deserializedObject = Schema.Deserialize(dataBuffer);

            return new HeaderWithValue<T>
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public HeaderWithValue<T> Decode(DataBuffer dataBuffer, T existing)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T>(header, default);
            }

            T? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new HeaderWithValue<T>
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        public void Encode(DataBuffer dataBuffer, T? oldObject, T? newObject)
        {
            Header header = new() { IsMissing = false, IsNull = newObject == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            Schema.Serialize(oldObject!, newObject!, dataBuffer);
        }

        public int GetLengthInBytes(T value)
        {
            return Schema.GetLengthInBytes(value);
        }

        public int GetLengthInBits(T? value)
        {
            return ((ISchema)Schema).GetLengthInBits(value);
        }

        public int GetLengthInBits(T? oldValue, T? newValue)
        {
            return ((ISchema)Schema).GetLengthInBits(oldValue, newValue);
        }
    }
}