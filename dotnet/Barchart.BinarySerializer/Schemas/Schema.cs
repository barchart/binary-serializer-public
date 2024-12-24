#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Schemas.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <inheritdoc />
public class Schema<TEntity> : ISchema<TEntity> where TEntity : class, new()
{
    #region Fields

    private readonly ISchemaItem<TEntity>[] _keyItems;
    private readonly ISchemaItem<TEntity>[] _valueItems;

    private readonly BinaryHeaderSerializer _headerSerializer;

    #endregion

    #region Properties

    /// <inheritdoc />
    public byte EntityId { get; }

    #endregion

    #region Constructor(s)

    /// <inheritdoc cref="Schema{TEntity}(byte, ISchemaItem{TEntity}[])"/>
    public Schema(ISchemaItem<TEntity>[] items) : this(0, items)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Schema{TEntity}" /> class.
    /// </summary>
    /// <param name="entityId">
    ///     The entity ID to be included in the header. This ID helps identify the type of entity
    /// </param>
    /// <param name="items">
    ///     The schema items that describe how to serialize and deserialize the entity.
    /// </param>
    public Schema(byte entityId, ISchemaItem<TEntity>[] items)
    {
        EntityId = entityId;

        _keyItems = items.Where(i => i.Key).ToArray();
        _valueItems = items.Where(i => !i.Key).ToArray();

        _headerSerializer = BinaryHeaderSerializer.Instance;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public byte[] Serialize(IDataBufferWriter writer, TEntity source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        if (writer.BytesWritten == 0)
        {
            _headerSerializer.Encode(writer, EntityId, true);
        }

        foreach (ISchemaItem<TEntity> item in _keyItems)
        {
            item.Encode(writer, source);
        }

        foreach (ISchemaItem<TEntity> item in _valueItems)
        {
            item.Encode(writer, source);
        }

        return writer.ToBytes();
    }

    /// <inheritdoc />
    public byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        if (writer.BytesWritten == 0)
        {
            _headerSerializer.Encode(writer, EntityId, false);
        }

        foreach (ISchemaItem<TEntity> item in _keyItems)
        {
            item.Encode(writer, current, previous);
        }

        foreach (ISchemaItem<TEntity> item in _valueItems)
        {
            item.Encode(writer, current, previous);
        }

        return writer.ToBytes();
    }

    /// <inheritdoc />
    public TEntity Deserialize(IDataBufferReader reader)
    {
        return Deserialize(reader, new TEntity(), false);
    }

    /// <inheritdoc />
    public TEntity Deserialize(IDataBufferReader reader, TEntity target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
            
        return Deserialize(reader, target, true);
    }

    private TEntity Deserialize(IDataBufferReader reader, TEntity target, bool existing)
    {
        if (reader.BytesRead == 0)
        {
            Header header = ReadHeader(reader);
            CheckHeader(header);
        }

        foreach (ISchemaItem<TEntity> item in _keyItems)
        {
            item.Decode(reader, target, existing);
        }

        foreach (ISchemaItem<TEntity> item in _valueItems)
        {
            item.Decode(reader, target, existing);
        }

        return target;
    }

    /// <inheritdoc />
    public Header ReadHeader(IDataBufferReader reader)
    {
        return _headerSerializer.Decode(reader);
    }

    /// <inheritdoc />
    public TMember ReadKey<TMember>(IDataBufferReader reader, string name)
    {
        using (reader.Bookmark())
        {
            Header header = ReadHeader(reader);
            CheckHeader(header);

            TEntity target = new();

            for (int i = 0; i < _keyItems.Length; i++)
            {
                ISchemaItem<TEntity> candidate = _keyItems[i];

                candidate.Decode(reader, target);

                if (candidate.Name == name && candidate is ISchemaItem<TEntity, TMember> match)
                {
                    return match.Read(target);
                }
            }
        }

        throw new KeyUndefinedException(typeof(TEntity), name, typeof(TMember));
    }
        
    /// <inheritdoc />
    public bool GetEquals(TEntity? a, TEntity? b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a != null && b != null)
        {
            return _keyItems.All(si => si.GetEquals(a, b)) && _valueItems.All(si => si.GetEquals(a, b));
        }

        return false;
    }
        
    private void CheckHeader(Header header)
    {
        if (header.EntityId != EntityId)
        {
            throw new HeaderMismatchException(header.EntityId, EntityId);
        }
    }

    #endregion
}