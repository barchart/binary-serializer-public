namespace JerqAggregatorNew.Schemas
{
    internal interface ISchema
	{
        public byte[] Serialize(object schemaObject);

        public byte[] Serialize(object schemaObject, byte[] buffer);

        internal byte[] Serialize(object schemaObject, BufferHelper bufferHelper);

        public byte[] Serialize(object oldObject, object newObject);

        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);

        internal byte[] Serialize(object oldObject, object newObject, BufferHelper bufferHelper);

        public object? Deserialize(byte[] buffer);

        internal object? Deserialize(BufferHelper bufferHelper);

        public object? Deserialize(byte[] buffer, object existing);

        internal object? Deserialize(object existing, BufferHelper bufferHelper);

        public int GetLengthInBytes(object? schemaObject);

        public int GetLengthInBits(object? schemaObject);

    }
}

