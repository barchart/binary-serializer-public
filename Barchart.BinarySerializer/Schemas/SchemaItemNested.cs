#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Utilities;

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

    private readonly Func<TEntity, TMember> _getter;
    private readonly Action<TEntity, TMember> _setter;

    private readonly ISchema<TMember> _schema;
    
    #endregion

    #region Properties
    
    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Key { get; }
    
    #endregion

    #region Constructor(s)

    public SchemaItemNested(string name, Func<TEntity, TMember> getter, Action<TEntity, TMember> setter, ISchema<TMember> schema)
    {
        Name = name;
        Key = false;
        
        _getter = getter;
        _setter = setter;

        _schema = schema;
    }

    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        TMember nested = _getter(source);
        
        Serialization.WriteMissingFlag(writer, false);
        Serialization.WriteNullFlag(writer, nested == null);

        if (nested != null)
        {
            _schema.Serialize(writer, nested, true);
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        bool unchanged = GetEquals(current, previous);
        
        Serialization.WriteMissingFlag(writer, unchanged);
        
        if (unchanged)
        {
            return;
        }
        
        TMember nestedCurrent = _getter(current);
        TMember nestedPrevious = _getter(previous);
        
        Serialization.WriteNullFlag(writer, nestedCurrent == null);

        if (nestedCurrent == null)
        {
            return;
        }
        
        if (nestedPrevious == null)
        {
            _schema.Serialize(writer, nestedCurrent, true);

        }
        else
        { 
            _schema.Serialize(writer, nestedCurrent, nestedPrevious, true);
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        if (Serialization.ReadMissingFlag(reader))
        {
            return;
        }

        TMember nested = _getter(target);
        
        if (Serialization.ReadNullFlag(reader))
        {
            if (nested != null)
            {
                _setter(target, null!);
            }
        } 
        else if (nested == null)
        {
            _setter(target, _schema.Deserialize(reader, true));
        }
        else
        {
            _schema.Deserialize(reader, nested, true);
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
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
    
    #endregion
}