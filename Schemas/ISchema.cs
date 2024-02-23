namespace JerqAggregatorNew.Schemas
{
	public interface ISchema
	{
        public byte[] Serialize(object schemaObject);

        public byte[] Serialize(object schemaObject, byte[] buffer);

        public byte[] Serialize(object schemaObject, byte[] buffer, ref int offset, ref int offsetInLastByte);

        public byte[] Serialize(object oldObject, object newObject);

        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);

        public object Deserialize(byte[] buffer);

        public object Deserialize(byte[] buffer, ref int offset, ref int offsetInLastByte);

        public object Deserialize(byte[] buffer, object existing);

    }
}

