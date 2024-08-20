#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Schemas.Headers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Serializes and deserializes instances of the <typeparamref name="TEntity"/> class.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity that can be serialized or deserialized.
/// </typeparam>
public interface ISchema<TEntity> where TEntity : class, new()
{
    /// <summary>
    ///     Serializes the <paramref name="source"/> entity. In other words,
    ///     this method creates a binary "snapshot" of the entity.
    /// </summary>
    /// <param name="writer">
    ///     A buffer for binary data, used during the serialization process.
    /// </param>
    /// <param name="source">
    ///     The entity to serialize.
    /// </param>
    /// <returns>
    ///     The serialized entity, as a byte array.
    /// </returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity source);

    /// <summary>
    ///     Serializes changes between the <paramref name="current"/> and
    ///     <paramref name="previous"/> versions of an entity. In other words,
    ///     this method creates a binary "delta" representing the state change
    ///     between two version of an entity.
    /// </summary>
    /// <param name="writer">
    ///     A buffer for binary data, used during the serialization process.
    /// </param>
    /// <param name="current">
    ///     The current version of the entity.
    /// </param>
    /// <param name="previous">
    ///     The previous version of the entity.
    /// </param>
    /// <exception cref="KeyMismatchException">
    ///     Thrown when the <paramref name="current"/> and <paramref name="previous"/>
    ///     instances have different key values.
    /// </exception>
    /// <returns>
    ///     The serialized changes to the entity, as a byte array.
    /// </returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous);

    /// <summary>
    ///     Serializes a Header into the provided data buffer writer.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer to which the header will be written.
    /// </param>
    /// <param name="entityId">
    ///     The entity ID to be included in the header. This ID helps identify the type of entity
    ///     the data represents.
    /// </param>
    /// <param name="snapshot">
    ///     A boolean value indicating whether the data represents a snapshot. If true, the
    ///     snapshot flag will be set in the header.
    /// </param>
    /// <returns>
    ///     The serialized header, as a byte.
    /// </returns>
    byte SerializeHeader(IDataBufferWriter writer, byte entityId, bool snapshot);

    /// <summary>
    ///     Deserializes an entity. In other words, this method recreates the serialized
    ///     "snapshot" as a new instance of the <typeparamref name="TEntity"/> class.
    /// </summary>
    /// <param name="reader">
    ///     A buffer of binary data which contains the serialized entity.
    /// </param>
    /// <returns>
    ///     A new instance of the <typeparamref name="TEntity"/> class.
    /// </returns>
    TEntity Deserialize(IDataBufferReader reader);

    /// <summary>
    ///     Deserializes an entity, updating an existing instance of
    ///     the <typeparamref name="TEntity"/> class.
    /// </summary>
    /// <param name="reader">
    ///     A buffer of binary data which contains the serialized entity.
    /// </param>
    /// <param name="target">
    ///     The target entity to assign the deserialized values to.
    /// </param>
    /// <exception cref="KeyMismatchException">
    ///     Thrown when key, read from the serialized binary data, does not
    ///     match the key of the <paramref name="target"/> instance.
    /// </exception>
    /// <returns>
    ///     The reference to the <paramref name="target"/> instance.
    /// </returns>
    TEntity Deserialize(IDataBufferReader reader, TEntity target);

    /// <summary>
    ///     Deserializes a Header from the provided data buffer reader.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader from which the header will be read.
    /// </param>
    /// <returns>
    ///     An <see cref="IHeader"/> instance representing the decoded Header. The Header includes
    ///     information such as the entity ID and whether the data is a snapshot.
    /// </returns>
    IHeader DeserializeHeader(IDataBufferReader reader);

    /// <summary>
    ///     Deserializes a key value (only) from the <paramref name="reader" />.
    /// </summary>
    /// <param name="reader">
    ///     The serialized data source from which to read the key.
    /// </param>
    /// <param name="name">
    ///     The name of the key property.
    /// </param>
    /// <typeparam name="TProperty">
    ///     The type of the key property.
    /// </typeparam>
    /// <returns>
    ///     The value of the key.
    /// </returns>
    TProperty ReadKey<TProperty>(IDataBufferReader reader, string name);
    
    /// <summary>
    ///     Compares two objects and applies non-null fields from the source object to the target object.
    /// </summary>
    /// <param name="target">
    ///     The object to be updated.
    /// </param>
    /// <param name="source">
    ///     The object containing the updates.
    /// </param>
    void CompareAndUpdate(ref TEntity target, TEntity source);
    
    /// <summary>
    ///     Performs a deep equality check of two <typeparamref name="TEntity"/>
    ///     instances.
    /// </summary>
    /// <param name="a">
    ///     The first entity.
    /// </param>
    /// <param name="b">
    ///     The second entity.
    /// </param>
    /// <returns>
    ///     True, if the serializable members of the instances are equal;
    ///     otherwise false.
    /// </returns>
    bool GetEquals(TEntity a, TEntity b);
}