#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Defines a schema for serialization and deserialization of entities.
/// </summary>
/// <typeparam name="TEntity">
///     The type of entity this schema is for.
/// </typeparam>
public interface ISchema<TEntity> where TEntity: new()
{
    #region Methods

    /// <summary>
    ///     Serializes the given entity into a byte array.
    /// </summary>
    /// <param name="source">
    ///     The entity to serialize.
    /// </param>
    /// <param name="writer">
    ///     The buffer writer to use for serialization.
    /// </param>
    /// <returns>
    ///     A byte array containing the serialized entity data.
    /// </returns>
    byte[] Serialize(TEntity source, IDataBufferWriter writer);

    /// <summary>
    ///     Deserializes data into the given entity.
    /// </summary>
    /// <param name="target">
    ///     The entity to populate with deserialized data.
    /// </param>
    /// <param name="reader">
    ///     The buffer reader to use for deserialization.
    /// </param>
    void Deserialize(TEntity target, IDataBufferReader reader);

    #endregion
}