#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Represents a schema for serializing and deserializing objects of type <typeparam name="TEntity" />.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the object to be serialized and deserialized.
/// </typeparam>
public interface ISchema<TEntity> where TEntity : new()
{
    #region Methods

    /// <summary>
    ///     Serializes the specified source object into a byte array using the provided data buffer writer.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer used for serialization.
    /// </param>
    /// <param name="source">
    ///     The source object to be serialized.
    /// </param>
    /// <returns>
    ///     A byte array representing the serialized object.
    /// </returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity source);

    /// <summary>
    ///     Serializes the specified current and previous objects into a byte array using the provided data buffer writer.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer used for serialization.
    /// </param>
    /// <param name="current">
    ///     The current object to be serialized.</param>
    /// <param name="previous">
    ///     The previous object to be serialized.
    /// </param>
    /// <returns>
    ///     A byte array representing the serialized objects.
    /// </returns>
    byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous);

    /// <summary>
    ///     Deserializes an object of type TEntity from the specified data buffer reader.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader used for deserialization.
    /// </param>
    /// <returns>
    ///     The deserialized object of type TEntity.
    /// </returns>
    TEntity Deserialize(IDataBufferReader reader);

    /// <summary>
    ///     Deserializes an object of type TEntity from the specified data buffer reader into the provided target object.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader used for deserialization.
    /// </param>
    /// <param name="target">
    ///     The target object to be populated with the deserialized data.
    /// </param>
    /// <returns>
    ///     The deserialized object of type TEntity.
    /// </returns>
    TEntity Deserialize(IDataBufferReader reader, TEntity target);

    #endregion
}