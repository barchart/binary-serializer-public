#region Using Statements

using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Serializers;

/// <summary>
///     A builder class for constructing instances of <see cref="Serializer{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">
///     The type of entity to be (de)serialized.
/// </typeparam>
public class SerializerBuilder<TEntity> where TEntity: class, new()
{
    #region Fields

    private ISchemaFactory _schemaFactory;

    private IDataBufferReaderFactory _dataBufferReaderFactory;
    private IDataBufferWriterFactory _dataBufferWriterFactory;

    private readonly byte _entityId;
    
    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Initializes a new instance of the <see cref="SerializerBuilder{TEntity}"/> class 
    ///     with default factories for schema, data buffer reader, and data buffer writer.
    /// </summary>
    /// <param name="entityId">
    ///     The entity ID to be assigned to the schema.
    /// </param>
    public SerializerBuilder(byte entityId = 0)
    {
        _schemaFactory = new SchemaFactory();
        
        _dataBufferReaderFactory = new DataBufferReaderFactory();
        _dataBufferWriterFactory = new DataBufferWriterFactory();

        _entityId = entityId;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Specifies the schema factory.
    /// </summary>
    /// <param name="schemaFactory">
    ///     The schema factory.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="SerializerBuilder{TEntity}"/> for method chaining.
    /// </returns>
    public SerializerBuilder<TEntity> WithSchemaFactory(ISchemaFactory schemaFactory)
    {
        _schemaFactory = schemaFactory;
        
        return this;
    }

    /// <summary>
    ///     Specifies the binary type factory.
    /// </summary>
    /// <param name="typeSerializerFactory">
    ///     The binary type serializer factory.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="SerializerBuilder{TEntity}"/> for method chaining.
    /// </returns>
    public SerializerBuilder<TEntity> WithSchemaFactory(IBinaryTypeSerializerFactory typeSerializerFactory)
    {
        _schemaFactory = new SchemaFactory(typeSerializerFactory);
        
        return this;
    }

    /// <summary>
    ///     Specifies the data buffer reader factory to be used by the serializer.
    /// </summary>
    /// <param name="dataBufferReaderFactory">
    ///     The data buffer reader factory to be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="SerializerBuilder{TEntity}"/> for method chaining.
    /// </returns>
    public SerializerBuilder<TEntity> WithDataBufferReaderFactory(IDataBufferReaderFactory dataBufferReaderFactory)
    {
        _dataBufferReaderFactory = dataBufferReaderFactory;
        
        return this;
    }

    /// <summary>
    ///     Specifies the data buffer writer factory to be used by the serializer.
    /// </summary>
    /// <param name="dataBufferWriterFactory">
    ///     The data buffer writer factory to be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="SerializerBuilder{TEntity}"/> for method chaining.
    /// </returns>
    public SerializerBuilder<TEntity> WithDataBufferWriterFactory(IDataBufferWriterFactory dataBufferWriterFactory)
    {
        _dataBufferWriterFactory = dataBufferWriterFactory;
        
        return this;
    }

    /// <summary>
    ///     Builds and returns an instance of <see cref="Serializer{TEntity}"/>.
    /// </summary>
    /// <returns>
    ///     An instance of <see cref="Serializer{TEntity}"/> configured with the specified factories.
    /// </returns>
    public Serializer<TEntity> Build()
    {
        ISchema<TEntity> schema = _schemaFactory.Make<TEntity>(_entityId);
        
        return new Serializer<TEntity>(schema, _dataBufferReaderFactory, _dataBufferWriterFactory);
    }
    
    /// <summary>
    ///     Creates a new instance of <see cref="SerializerBuilder{TEntity}"/> for the specified entity type.
    /// </summary>
    /// <param name="entityId">
    ///     The entity ID to be assigned to the schema.
    /// </param>
    /// <returns>
    ///     A new instance of <see cref="SerializerBuilder{TEntity}"/>.
    /// </returns>
    public static SerializerBuilder<TEntity> ForType(byte entityId = 0)
    {
        return new SerializerBuilder<TEntity>(entityId);
    }

    #endregion
}
