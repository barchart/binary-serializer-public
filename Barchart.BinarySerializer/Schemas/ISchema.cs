#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Represents a schema for serializing and deserializing entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity.
/// </typeparam>
public interface ISchema<TEntity> where TEntity : class, new()
{
    /// <summary>
    ///     Serializes the <paramref name="source"/> entity to a byte array using the specified <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer.
    /// </param>
    /// <param name="source">
    ///     The entity to serialize.
    /// </param>
    /// <returns>The serialized byte array.</returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity source);

    /// <summary>
    ///     Deserializes the changes between the <paramref name="current"/> and <paramref name="previous"/> entities to a byte array using the specified <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer.
    /// </param>
    /// <param name="current">
    ///     The current entity.
    /// </param>
    /// <param name="previous">
    ///     The previous entity.
    /// </param>
    /// <returns>
    ///     The serialized byte array.
    /// </returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous);

    /// <summary>
    ///     Deserializes an entity of type <typeparamref name="TEntity"/> from the specified <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader.
    /// </param>
    /// <returns>
    ///     The deserialized entity.
    /// </returns>
    TEntity Deserialize(IDataBufferReader reader);

    /// <summary>
    ///     Deserializes an entity of type <typeparamref name="TEntity"/> from the specified <paramref name="reader"/> and assigns it to the <paramref name="target"/> entity.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader.
    /// </param>
    /// <param name="target">
    ///     The target entity to assign the deserialized values to.
    /// </param>
    /// <returns>
    ///     The deserialized entity.
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
    TProperty ReadKey<TProperty>(IDataBufferReader reader, string name);

    /// <summary>
    ///     Determines whether two entities of type <typeparamref name="TEntity"/> are equal.
    /// </summary>
    /// <param name="a">
    ///     The first entity.
    /// </param>
    /// <param name="b">
    ///     The second entity.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the entities are equal; otherwise, <c>false</c>.
    /// </returns>
    bool GetEquals(TEntity a, TEntity b);
}