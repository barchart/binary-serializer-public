#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Provides a mechanism for handling the serialization and deserialization of nested properties within an entity,
///     facilitating the conversion to and from a binary format while managing nullability and presence flags for optimized storage efficiency.
/// </summary>
/// <typeparam name="TEntity">
///     The type which contains the data to be serialized. In other words,
///     this is the source of data being serialized (or the assignment
///     target of data being deserialized).
/// </typeparam>
/// <typeparam name="TProperty">
///     The type of data being serialized (which is read from the source
///     object) or deserialized (which assigned to the source object).
/// </typeparam>
public class SchemaItemNested<TEntity, TProperty> : ISchemaItem<TEntity> where TEntity: class, new() where TProperty: class, new()
{
    #region Fields

    private readonly string _name;

    private readonly Func<TEntity, TProperty> _getter;
    private readonly Action<TEntity, TProperty> _setter;

    private readonly ISchema<TProperty> _schema;
    
    #endregion

    #region Constructor(s)

    public SchemaItemNested(string name, Func<TEntity, TProperty> getter, Action<TEntity, TProperty> setter, ISchema<TProperty> schema)
    {
        _name = name;

        _getter = getter;
        _setter = setter;

        _schema = schema;
    }

    #endregion
    
    #region Properties
    
    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => false;
    
    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        TProperty nested = _getter(source);
        
        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, nested == null);

        if (nested != null)
        {
            _schema.Serialize(writer, nested);
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        if (GetEquals(current, previous))
        {
            WriteMissingFlag(writer, true);

            return;
        }
        
        TProperty nestedCurrent = _getter(current);
        TProperty nestedPrevious = _getter(previous);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, nestedCurrent == null);

        if (nestedCurrent != null)
        {
            _schema.Serialize(writer, nestedCurrent, nestedPrevious);
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        if (ReadMissingFlag(reader))
        {
            return;
        }

        TProperty nested = _getter(target);
        
        if (ReadNullFlag(reader))
        {
            if (nested != null)
            {
                _setter(target, null!);
            }

            return;
        }
        
        if (nested == null)
        {
            _setter(target, _schema.Deserialize(reader));
        }
        else
        {
            _schema.Deserialize(reader, nested);
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        return _schema.GetEquals(_getter(a), _getter(b));
    }
    
    private static bool ReadMissingFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    private static void WriteMissingFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }

    private static bool ReadNullFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    private static void WriteNullFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }
    
    #endregion
}