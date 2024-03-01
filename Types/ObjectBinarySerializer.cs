using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class ObjectBinarySerializer : ISerializer
    {
        internal ISchema Schema { get; set; }
       
        internal ObjectBinarySerializer(ISchema schema)
        {
            Schema = schema;
        }

        public HeaderWithValue Decode(DataBuffer dataBuffer)
        {
            Header header = new Header();

            header.IsMissing = dataBuffer.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = Schema.Deserialize(dataBuffer);

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public HeaderWithValue Decode(DataBuffer dataBuffer, object? existing)
        {
            Header header = new Header();

            header.IsMissing = dataBuffer.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = null;

            if (existing != null)
            {
                deserializedObject = Schema.Deserialize(existing, dataBuffer);
            }

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(DataBuffer dataBuffer, object? value)
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

        public void Encode(DataBuffer dataBuffer, object? oldObject, object? newObject)
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

        public int GetLengthInBytes(object? value)
        {
            return Schema.GetLengthInBytes(value);
        }

        public int GetLengthInBits(object? value)
        {
            return Schema.GetLengthInBits(value);
        }
    }
}

