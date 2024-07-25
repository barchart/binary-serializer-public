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

    private readonly SchemaFactory _schemaFactory;
    private readonly ISchema<TEntity> _schema;

    private readonly DataBufferWriterFactory _writerFactory;
    private readonly IDataBufferWriter _writer;

    #endregion

    #region Constructor(s)

    public Serializer()
    {
        _schemaFactory = new();
        _schema = _schemaFactory.Make<TEntity>();

        _writerFactory = new();
        _writer = _writerFactory.Make();
    }

    #endregion

    #region Methods

    public byte[] Serialize(TEntity source)
    {
        using(_writer.Bookmark())
        {
            return _schema.Serialize(_writer, source);
        }
    }

    public byte[] Serialize(TEntity current, TEntity previous)
    {
        using(_writer.Bookmark())
        {
            return _schema.Serialize(_writer, current, previous);
        }
    }

    public TEntity Deserialize(byte[] serialized)
    {
        DataBufferReader reader = new(serialized);

        return _schema.Deserialize(reader);
    }

    public TEntity Deserialize(byte[] serialized, TEntity target)
    {
        DataBufferReader reader = new(serialized);

        return _schema.Deserialize(reader, target);
    }

    #endregion
}