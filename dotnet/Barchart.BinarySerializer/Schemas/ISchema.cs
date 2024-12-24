#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers;
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
    #region Properties
    
    /// <summary>
    ///     The entity ID to be included in the header. This ID helps identify the type of entity
    ///     the data represents.
    /// </summary>
    byte EntityId { get; }

    #endregion

    #region Methods

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
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="source"/> entity is null.
    /// </exception>
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
    /// <returns>
    ///     The serialized changes to the entity, as a byte array.
    /// </returns>
    /// <exception cref="KeyMismatchException">
    ///     Thrown when the <paramref name="current"/> and <paramref name="previous"/>
    ///     instances have different key values.
    /// </exception>
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
    /// <exception cref="HeaderMismatchException">
    ///     Thrown when the header from serialized the byte array is invalid.
    /// </exception>
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
    /// <returns>
    ///     The reference to the <paramref name="target"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="target"/> entity is null.
    /// </exception>
    /// <exception cref="HeaderMismatchException">
    ///     Thrown when the header from serialized the byte array is invalid.
    /// </exception>
    /// <exception cref="KeyMismatchException">
    ///     Thrown when key, read from the serialized binary data, does not
    ///     match the key of the <paramref name="target"/> instance.
    /// </exception>
    TEntity Deserialize(IDataBufferReader reader, TEntity target);

    /// <summary>
    ///     Deserializes a byte array into a <see cref="Header"/> instance.
    /// </summary>
    /// <param name="reader">
    ///     A buffer of binary data which contains the serialized header.
    /// </param>
    /// <returns>
    ///     A <see cref="Header"/> instance representing the decoded header, which includes metadata such as the entity ID and snapshot information.
    /// </returns>
    public Header ReadHeader(IDataBufferReader reader);
    
    /// <summary>
    ///     Deserializes a key value (only) from the <paramref name="reader" />.
    /// </summary>
    /// <typeparam name="TMember">
    ///     The type of the key property.
    /// </typeparam>
    /// <param name="reader">
    ///     The serialized data source from which to read the key.
    /// </param>
    /// <param name="name">
    ///     The name of the key property.
    /// </param>
    /// <returns>
    ///     The value of the key.
    /// </returns>
    /// <exception cref="HeaderMismatchException">
    ///     Thrown when the header from serialized the byte array is invalid.
    /// </exception>
    /// <exception cref="KeyUndefinedException">
    ///     Thrown when the key property is not found in the schema.
    /// </exception>
    TMember ReadKey<TMember>(IDataBufferReader reader, string name);
    
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
    bool GetEquals(TEntity? a, TEntity? b);
    
    #endregion
}