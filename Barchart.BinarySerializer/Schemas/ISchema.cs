#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

public interface ISchema<TEntity> where TEntity: class, new()
{
    byte[] Serialize(IDataBufferWriter writer, TEntity source);

    byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous);

    TEntity Deserialize(IDataBufferReader reader);
    
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
    
    bool GetEquals(TEntity a, TEntity b);
}