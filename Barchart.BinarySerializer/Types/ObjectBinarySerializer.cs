using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for objects of type <typeparamref name="T"/> using a specified schema.
    /// </summary>
    /// <typeparam name="T">The type of objects to be serialized.</typeparam>
    public class ObjectBinarySerializer<T> : IBinaryTypeSerializer<T> where T : new()
    {
        public Schema<T> Schema { get; set; }

        public ObjectBinarySerializer(Schema<T> schema)
        {
            Schema = schema;
        }

        public HeaderWithValue<T> Decode(DataBuffer dataBuffer)
        {
            Header header = new Header();

            header.IsMissing = dataBuffer.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue<T>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
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
            Header header = new Header();

            header.IsMissing = dataBuffer.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue<T>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue<T>(header, default);
            }

            T? deserializedObject = default;

            if (existing != null)
            {
                deserializedObject = Schema.Deserialize(existing, dataBuffer);
            }

            return new HeaderWithValue<T>
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        public void Encode(DataBuffer dataBuffer, T oldObject, T newObject)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = newObject == null;

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

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
            return((ISchema)Schema).GetLengthInBits(value);
        }

        public int GetLengthInBits(T? oldValue, T? newValue)
        {
            return ((ISchema)Schema).GetLengthInBits(oldValue, newValue);
        }
    }
}