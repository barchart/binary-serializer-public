using JerqAggregatorNew.Schemas;

namespace JerqAggregatorNew.Types
{
    public class ObjectBinarySerializer : ISerializer
    {
        public ISchema Schema { get; set; }
       
        public ObjectBinarySerializer(ISchema schema)
        {
            Schema = schema;
        }

        public HeaderWithValue Decode(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();

            header.IsMissing = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = Schema.Deserialize(buffer, ref offset, ref offsetInLastByte);

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public HeaderWithValue Decode(byte[] buffer, object? existing, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();

            header.IsMissing = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsMissing)
            {
                return new HeaderWithValue(header, null);
            }

            header.IsNull = buffer.ReadBit(ref offset, ref offsetInLastByte) == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue(header, null);
            }

            object? deserializedObject = null;

            if (existing != null)
            {
                deserializedObject = Schema.Deserialize(buffer, existing, ref offset, ref offsetInLastByte);
            }

            return new HeaderWithValue
            {
                Header = header,
                Value = deserializedObject
            };
        }

        public void Encode(byte[] buffer, object? value, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = value == null;

            buffer.WriteBit(0, ref offset, ref offsetInLastByte);
            buffer.WriteBit((byte)(header.IsNull ? 1 : 0), ref offset, ref offsetInLastByte);

            if (value != null)
            {
                Schema.Serialize(value, buffer, ref offset, ref offsetInLastByte);
            }
        }

        public void Encode(byte[] buffer, object? oldObject, object? newObject, ref int offset, ref int offsetInLastByte)
        {
            Header header = new Header();
            header.IsMissing = false;
            header.IsNull = newObject == null;

            buffer.WriteBit(0, ref offset, ref offsetInLastByte);
            buffer.WriteBit((byte)(header.IsNull ? 1 : 0), ref offset, ref offsetInLastByte);

            if (oldObject != null && newObject != null)
            {
                Schema.Serialize(oldObject, newObject, buffer, ref offset, ref offsetInLastByte);
            }
        }

        public int GetLengthInBytes(object? value)
        {
            throw new NotImplementedException();
        }
    }
}

