namespace Barchart.BinarySerializer.Schemas
{
    public interface ISchema
	{
        public byte[] Serialize(object schemaObject);
        public byte[] Serialize(object schemaObject, byte[] buffer);
        internal byte[] Serialize(object schemaObject, DataBuffer bufferHelper);
        public byte[] Serialize(object oldObject, object newObject);
        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);
        internal byte[] Serialize(object oldObject, object newObject, DataBuffer bufferHelper);

        public object? Deserialize(byte[] buffer);
        internal object? Deserialize(DataBuffer bufferHelper);
        public object? Deserialize(byte[] buffer, object existing);
        internal object? Deserialize(object existing, DataBuffer bufferHelper);

        public int GetLengthInBytes(object? schemaObject);
        public int GetLengthInBits(object? schemaObject);
    }
}

