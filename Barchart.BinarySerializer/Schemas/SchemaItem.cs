#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Information regarding a single piece of data that can be serialized
///     to a binary storage (or deserialized from binary data storage).
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
public class SchemaItem<TEntity, TMember> : ISchemaItem<TEntity, TMember> where TEntity: class, new()
{
    #region Fields

    private readonly Func<TEntity, TMember> _getter;
    private readonly Action<TEntity, TMember> _setter;

    private readonly IBinaryTypeSerializer<TMember> _serializer;
    
    #endregion

    #region Properties
    
    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Key { get; }

    #endregion

    #region Constructor(s)

    public SchemaItem(string name, bool key, Func<TEntity, TMember> getter, Action<TEntity, TMember> setter, IBinaryTypeSerializer<TMember> serializer)
    {
        Name = name;
        Key = key;

        _getter = getter;
        _setter = setter;
        
        _serializer = serializer;
    }

    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source) 
    {
        if (!Key)
        {
            Serialization.WriteMissingFlag(writer, false);
        }

        _serializer.Encode(writer, _getter(source));
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        bool valuesAreEqual = GetEquals(current, previous);
        
        if (Key && !valuesAreEqual)
        {
            throw new KeyMismatchException(typeof(TEntity), Name, true);
        }

        if (Key || !valuesAreEqual)
        {
            Encode(writer, current);
        }
        else
        {
            Serialization.WriteMissingFlag(writer, true); 
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        bool missing;

        if (Key)
        {
            missing = false;
        }
        else
        {
            missing = Serialization.ReadMissingFlag(reader);
        }

        if (missing)
        {
            return;
        }
        
        TMember current = _serializer.Decode(reader);

        if (Key && existing)
        {
            if (!_serializer.GetEquals(current, _getter(target)))
            {
                throw new KeyMismatchException(typeof(TEntity), Name, false);
            }
        }
        else
        {
            _setter(target, current);
        }
    }

    /// <inheritdoc />
    public void CompareAndApply(ref TEntity target, TEntity source)
    {
        if (source == null)
        {
            return;
        }
        
        target ??= new TEntity();
        
        TMember sourceValue = _getter(source);
        
        if (sourceValue == null)
        {
            return;
        }

        _setter(target, sourceValue);
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a != null && b != null)
        {
            return _serializer.GetEquals(_getter(a), _getter(b));
        }
        
        return false;
    }
    
    /// <inheritdoc />
    public TMember Read(TEntity source)
    {
        return _getter(source);
    }
    
    #endregion
}