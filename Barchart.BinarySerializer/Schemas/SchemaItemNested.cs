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
/// <typeparam name="TMember">
///     The type of data being serialized (which is read from the source
///     object) or deserialized (which assigned to the source object).
/// </typeparam>
public class SchemaItemNested<TEntity, TMember> : ISchemaItem<TEntity> where TEntity: class, new() where TMember: class, new()
{
    #region Fields

    private readonly string _name;

    private readonly Func<TEntity, TMember?> _getter;
    private readonly Action<TEntity, TMember?> _setter;

    private readonly ISchema<TMember> _schema;
    
    #endregion

    #region Properties
    
    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => false;
    
    #endregion

    #region Constructor(s)

    public SchemaItemNested(string name, Func<TEntity, TMember?> getter, Action<TEntity, TMember?> setter, ISchema<TMember> schema)
    {
        _name = name;

        _getter = getter;
        _setter = setter;

        _schema = schema;
    }

    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        TMember? nested = _getter(source);
        
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
        
        TMember? nestedCurrent = _getter(current);
        TMember? nestedPrevious = _getter(previous);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, nestedCurrent == null);

        if (nestedCurrent == null)
        {
            return;
        }
        
        if (nestedPrevious == null)
        {
            _schema.Serialize(writer, nestedCurrent);

        }
        else
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

        TMember? nested = _getter(target);
        
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
    public void CompareAndApply(ref TEntity? target, TEntity? source)
    {
        if (source == null)
        {
            return;
        }
        
        target ??= new TEntity();
        
        TMember? sourceValue = _getter(source);
        TMember? targetValue = _getter(target);

        if (sourceValue == null)
        {
            return;
        }
        
        if (targetValue == null)
        {
            _setter(target, sourceValue);
        }
        else
        {
            _schema.CompareAndUpdate(ref targetValue, sourceValue);
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity? a, TEntity? b)
    {
        if (a == null && b == null)
        {
            return true;
        }
        
        if (a == null || b == null)
        {
            return false;
        }
        
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