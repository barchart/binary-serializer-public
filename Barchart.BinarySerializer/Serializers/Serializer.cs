#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;

#endregion

namespace Barchart.BinarySerializer.Serializers;

/// <summary>
///     Provides (de)serialization functionality for entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
///     The type of entity to be (de)serialized.
/// </typeparam>
public class Serializer<TEntity> where TEntity : class, new()
{
    #region Fields

    private readonly ISchema<TEntity> _schema;
    
    private readonly IDataBufferReaderFactory _dataBufferReaderFactory;
    private readonly IDataBufferWriterFactory _dataBufferWriterFactory;

    #endregion
    
    #region Constructor(s)

    public Serializer()
    {
        ISchemaFactory schemaFactory = new SchemaFactory();
        
        _schema = schemaFactory.Make<TEntity>();
        
        _dataBufferReaderFactory = new DataBufferReaderFactory();
        _dataBufferWriterFactory = new DataBufferWriterFactory();
    }

    public Serializer(ISchema<TEntity> schema, IDataBufferReaderFactory dataBufferReaderFactory, IDataBufferWriterFactory dataBufferWriterFactory)
    {
        _schema = schema;
        
        _dataBufferReaderFactory = dataBufferReaderFactory;
        _dataBufferWriterFactory = dataBufferWriterFactory;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Serializes the <paramref name="source"/> entity. In other words,
    ///     this method creates a binary "snapshot" of the entity.
    /// </summary>
    /// <param name="source">
    ///     The entity to serialize.
    /// </param>
    /// <returns>
    ///     The serialized entity, as a byte array.
    /// </returns>
    public byte[] Serialize(TEntity source)
    {
        IDataBufferWriter writer = _dataBufferWriterFactory.Make();
        
        return _schema.Serialize(writer, source);
    }

    /// <summary>
    ///     Serializes changes between the <paramref name="current"/> and
    ///     <paramref name="previous"/> versions of an entity. In other words,
    ///     this method creates a binary "delta" representing the state change
    ///     between two version of an entity.
    /// </summary>
    /// <param name="current">
    ///     The current version of the entity.
    /// </param>
    /// <param name="previous">
    ///     The previous version of the entity.
    /// </param>
    /// <returns>
    ///     The serialized changes to the entity, as a byte array.
    /// </returns>
    public byte[] Serialize(TEntity current, TEntity previous)
    {
        IDataBufferWriter writer = _dataBufferWriterFactory.Make();
        
        return _schema.Serialize(writer, current, previous);
    }

    /// <summary>
    ///     Deserializes an entity. In other words, this method recreates the serialized
    ///     "snapshot" as a new instance of the <typeparamref name="TEntity"/> class.
    /// </summary>
    /// <param name="serialized">
    ///     The byte array that contains serialized data.
    /// </param>
    /// <returns>
    ///     A new instance of the <typeparamref name="TEntity"/> class.
    /// </returns>
    public TEntity Deserialize(byte[] serialized)
    {
        IDataBufferReader reader = _dataBufferReaderFactory.Make(serialized);
        
        return _schema.Deserialize(reader);
    }

    /// <summary>
    ///     Deserializes an entity, updating an existing instance of
    ///     the <typeparamref name="TEntity"/> class.
    /// </summary>
    /// <param name="serialized">
    ///     The byte array that contains serialized data.
    /// </param>
    /// <param name="target">
    ///     The target entity to populate with deserialized data.
    /// </param>
    /// <returns>
    ///     The reference to the <paramref name="target"/> instance.
    /// </returns>
    public TEntity Deserialize(byte[] serialized, TEntity target)
    {
        IDataBufferReader reader = _dataBufferReaderFactory.Make(serialized);
        
        return _schema.Deserialize(reader, target);
    }
    
    /// <summary>
    ///     Deserializes a key value (only) from the <paramref name="serialized" />.
    /// </summary>
    /// <param name="serialized">
    ///     A byte array containing the binary data to deserialize.
    /// </param>
    /// <param name="name">
    ///     The name of the key property
    /// </param>
    /// <typeparam name="TMember">
    ///     The type of the key property.
    /// </typeparam>
    /// <returns>
    ///     The value of the key.
    /// </returns>
    public TMember ReadKey<TMember>(byte[] serialized, string name)
    {
        IDataBufferReader reader = _dataBufferReaderFactory.Make(serialized);
        
        return _schema.ReadKey<TMember>(reader, name);
    }
    
    /// <summary>
    ///     Compares two objects and applies non-null fields from the source object to the target object.
    /// </summary>
    /// <param name="target">
    ///     The object to be updated.
    /// </param>
    /// <param name="source">
    ///     The object containing the updates.
    /// </param>
    public void CompareAndUpdate(ref TEntity target, TEntity source)
    {
        _schema.CompareAndUpdate(ref target, source);
    }

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
    public bool GetEquals(TEntity a, TEntity b)
    {
        return _schema.GetEquals(a, b);
    }

    #endregion
}