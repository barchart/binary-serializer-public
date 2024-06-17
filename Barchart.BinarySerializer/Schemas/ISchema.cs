namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Interface for defining custom serialization schemas.
    /// </summary>
    public interface ISchema
	{
        #region Methods

        public byte[] Serialize(object schemaObject);
        public byte[] Serialize(object schemaObject, byte[] buffer);
        internal byte[] Serialize(object schemaObject, DataBuffer dataBuffer);
        public byte[] Serialize(object oldObject, object newObject);
        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);
        internal byte[] Serialize(object oldObject, object newObject, DataBuffer dataBuffer);

        public object? Deserialize(byte[] buffer);
        internal object? Deserialize(DataBuffer dataBuffer);
        public object? Deserialize(byte[] buffer, object existing);
        internal object? Deserialize(object existing, DataBuffer dataBuffer);

        public int GetLengthInBytes(object? schemaObject);
        public int GetLengthInBytes(object? oldObject, object? newObject);
        public int GetLengthInBits(object? schemaObject);
        public int GetLengthInBits(object? oldObject, object? newObject);

        #endregion
    }
}