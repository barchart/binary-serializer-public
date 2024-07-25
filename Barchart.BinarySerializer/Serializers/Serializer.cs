#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Factories;

#endregion

namespace Barchart.BinarySerializer.Serializers;

public class Serializer<T> where T : class, new()
{
    #region Fields

    private readonly SchemaFactory _schemaFactory;
    private readonly ISchema<T> _schema;

    private readonly DataBufferWriterFactory _writerFactory;
    private readonly IDataBufferWriter _writer;

    #endregion

    #region Constructor(s)

    public Serializer()
    {
        _schemaFactory = new();
        _schema = _schemaFactory.Make<T>();

        _writerFactory = new();
        _writer = _writerFactory.Make();
    }

    #endregion

    #region Methods

    public byte[] Serialize(T source)
    {
        using(_writer.Bookmark())
        {
            return _schema.Serialize(_writer, source);
        }
    }

    public T Deserialize(byte[] serialized)
    {
        DataBufferReader reader = new(serialized);
        
        return _schema.Deserialize(reader);
    }

    #endregion
}