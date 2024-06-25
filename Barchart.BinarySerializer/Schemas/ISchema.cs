namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Interface for defining custom serialization schemas.
    /// </summary>
    public interface ISchema
	{
        #region Methods

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(object schemaObject);

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(object schemaObject, byte[] buffer);

        /// <summary>
        ///     Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(object oldObject, object newObject);

        /// <summary>
        ///     Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(object oldObject, object newObject, byte[] buffer);

        /// <summary>
        ///     Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <returns> Deserialized object written into newly created object of generic type. </returns>
        public object? Deserialize(byte[] buffer);

        /// <summary>
        ///     eserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <param name="existing">Existing generic object.</param>
        /// <returns> Deserialized object written into existing object of generic type. </returns>
        public object? Deserialize(byte[] buffer, object existing);

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bytes.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bytes. </returns>
        public int GetLengthInBytes(object? schemaObject);

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bytes.
        /// </summary>
        /// <param name="oldObject">The old schema object.</param>
        /// <param name="newObject">The new schema object.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bytes. </returns>
        public int GetLengthInBytes(object? oldObject, object? newObject);

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bits.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bits. </returns>
        public int GetLengthInBits(object? schemaObject);

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bits.
        /// </summary>
        /// <param name="oldObject">The old schema object to calculate the length for.</param>
        /// <param name="newObject">The new schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bits. </returns>
        public int GetLengthInBits(object? oldObject, object? newObject);

        #endregion
    }
}