#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;

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
    byte EntityId { get; }
    
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
    public TProperty ReadKey<TProperty>(IDataBufferReader reader, string name);
    
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