using Barchart.BinarySerializer.Schemas;

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
            Header header = ReadHeader(dataBuffer);

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
            Header header = ReadHeader(dataBuffer);

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
            WriteHeader(dataBuffer, value);

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        public void Encode(DataBuffer dataBuffer, T? oldObject, T? newObject)
        {
            WriteHeader(dataBuffer, newObject);

            if (oldObject != null && newObject != null)
            {
                Schema.Serialize(oldObject, newObject, dataBuffer);
            }
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

        private Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private Header WriteHeader(DataBuffer dataBuffer, T? value)
        {
            Header header = new()
            {
                IsMissing = false,
                IsNull = value == null
            };

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

            return header;
        }
    }
}