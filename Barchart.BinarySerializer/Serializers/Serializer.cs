#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;

#endregion

namespace Barchart.BinarySerializer.Serializers;

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

    public byte[] Serialize(TEntity source)
    {
        IDataBufferWriter writer = _dataBufferWriterFactory.Make();

        return _schema.Serialize(writer, source);
    }

    public byte[] Serialize(TEntity current, TEntity previous)
    {
        IDataBufferWriter writer = _dataBufferWriterFactory.Make();

        return _schema.Serialize(writer, current, previous);
    }

    public TEntity Deserialize(byte[] serialized)
    {
        IDataBufferReader reader = _dataBufferReaderFactory.Make(serialized);

        return _schema.Deserialize(reader);
    }

    public TEntity Deserialize(byte[] serialized, TEntity target)
    {
        IDataBufferReader reader = _dataBufferReaderFactory.Make(serialized);

        return _schema.Deserialize(reader, target);
    }

    #endregion
}