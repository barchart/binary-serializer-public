#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Types;

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
/// <typeparam name="TProperty">
///     The type of data being serialized (which is read from the source
///     object) or deserialized (which assigned to the source object).
/// </typeparam>
public class SchemaItem<TEntity, TProperty> : ISchemaItem<TEntity> where TEntity: class, new()
{
    #region Fields

    private readonly string _name;
    
    private readonly bool _key;

    private readonly Func<TEntity, TProperty> _getter;
    private readonly Action<TEntity, TProperty> _setter;

    private readonly IBinaryTypeSerializer<TProperty> _serializer;
    
    #endregion

    #region Constructor(s)

    public SchemaItem(string name, bool key, Func<TEntity, TProperty> getter, Action<TEntity, TProperty> setter, IBinaryTypeSerializer<TProperty> serializer)
    {
        _name = name;
        _key = key;

        _getter = getter;
        _setter = setter;
        
        _serializer = serializer;
    }

    #endregion

    #region Properties
    
    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => _key;
    
    #endregion
    
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source) 
    {
        if (!Key)
        {
            WriteMissingFlag(writer, false);
        }

        _serializer.Encode(writer, _getter(source));
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        bool valuesAreEqual = GetEquals(current, previous);
        
        if (Key && !valuesAreEqual)
        {
            throw new KeyMismatchException(Name, true);
        }
        
        if (Key || !valuesAreEqual)
        {
            Encode(writer, current);
        }
        else
        {
            WriteMissingFlag(writer, true); 
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
            missing = ReadMissingFlag(reader);
        }

        if (missing)
        {
            return;
        }
        
        TProperty current = _serializer.Decode(reader);

        if (Key && existing)
        {
            if (!_serializer.GetEquals(current, _getter(target)))
            {
                throw new KeyMismatchException(_name, false);
            }
        }
        else
        {
            _setter(target, current);
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        return _serializer.GetEquals(_getter(a), _getter(b));
    }
    
    private static bool ReadMissingFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    private static void WriteMissingFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }
    
    #endregion
}