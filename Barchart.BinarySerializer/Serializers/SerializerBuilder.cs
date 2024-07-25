using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Types.Factories;

namespace Barchart.BinarySerializer.Serializers;

public class SerializerBuilder<TEntity> where TEntity: class, new()
{
    private ISchemaFactory _schemaFactory;

    private IDataBufferReaderFactory _dataBufferReaderFactory;
    private IDataBufferWriterFactory _dataBufferWriterFactory;
    
    public SerializerBuilder()
    {
        _schemaFactory = new SchemaFactory();

        _dataBufferReaderFactory = new DataBufferReaderFactory();
        _dataBufferWriterFactory = new DataBufferWriterFactory();
    }
    
    public SerializerBuilder<TEntity> WithSchemaFactory(ISchemaFactory schemaFactory)
    {
        _schemaFactory = schemaFactory;

        return this;
    }
    
    public SerializerBuilder<TEntity> WithSchemaFactory(IBinaryTypeSerializerFactory typeSerializerFactory)
    {
        _schemaFactory = new SchemaFactory(typeSerializerFactory);

        return this;
    }
    
    public SerializerBuilder<TEntity> WithDataBufferReaderFactory(IDataBufferReaderFactory dataBufferReaderFactory)
    {
        _dataBufferReaderFactory = dataBufferReaderFactory;

        return this;
    }
    
    public SerializerBuilder<TEntity> WithDataBufferWriterFactory(IDataBufferWriterFactory dataBufferWriterFactory)
    {
        _dataBufferWriterFactory = dataBufferWriterFactory;

        return this;
    }

    public Serializer<TEntity> Build()
    {
        ISchema<TEntity> schema = _schemaFactory.Make<TEntity>();
        
        return new Serializer<TEntity>(schema, _dataBufferReaderFactory, _dataBufferWriterFactory);
    }
    
    public static SerializerBuilder<TEntity> ForType<TEntity>() where TEntity: class, new()
    {
        return new SerializerBuilder<TEntity>();
    }
}