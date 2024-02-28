using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    public class ObjectBinarySerializer : ISerializer
    {
        internal ISchema Schema { get; set; }
       
        internal ObjectBinarySerializer(ISchema schema)
        {
            Schema = schema;
        }

        public HeaderWithValue Decode(BufferHelper bufferHelper)
        {
            Header header = new Header();

            header.IsMissing = bufferHelper.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = bufferHelper.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = Schema.Deserialize(bufferHelper);

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public HeaderWithValue Decode(BufferHelper bufferHelper, object? existing)
        {
            Header header = new Header();

            header.IsMissing = bufferHelper.ReadBit() == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = bufferHelper.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = null;

            if (existing != null)
            {
                deserializedObject = Schema.Deserialize(existing, bufferHelper);
            }

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(BufferHelper bufferHelper, object? value)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            bufferHelper.WriteBit(0);
            bufferHelper.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value != null)
            {
                Schema.Serialize(value, bufferHelper);
            }
        }

        public void Encode(BufferHelper bufferHelper, object? oldObject, object? newObject)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = newObject == null;

            bufferHelper.WriteBit(0);
            bufferHelper.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (oldObject != null && newObject != null)
            {
                Schema.Serialize(oldObject, newObject, bufferHelper);
            }
        }

        public int GetLengthInBytes(object? value)
        {
            return GetLengthInBits(value);
        }

        public int GetLengthInBits(object? value)
        {
            return Schema.GetLengthInBits(value);
        }
    }
}

