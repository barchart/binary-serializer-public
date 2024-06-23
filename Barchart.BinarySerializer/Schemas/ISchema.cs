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
        public byte[] Serialize(object oldObject, object newObject);
        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);

        public object? Deserialize(byte[] buffer);
        public object? Deserialize(byte[] buffer, object existing);

        public int GetLengthInBytes(object? schemaObject);
        public int GetLengthInBytes(object? oldObject, object? newObject);
        public int GetLengthInBits(object? schemaObject);
        public int GetLengthInBits(object? oldObject, object? newObject);

        #endregion
    }
}