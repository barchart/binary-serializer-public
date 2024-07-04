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
public class SchemaItem<TEntity, TProperty> : ISchemaItem<TEntity> where TEntity: new()
{
    #region Fields

    private readonly string _name;
    
    private readonly bool _key;

    private readonly Func<TEntity, TProperty> _getter;
    private readonly Action<TEntity, TProperty> _setter;

    private readonly IBinaryTypeSerializer<TProperty> _serializer;
    
    #endregion
    
    #region Properties
    
    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => _key;
    
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

    #region Methods
    
    /// <inheritdoc />
    public void Encode(TEntity source, IDataBufferWriter writer) 
    {
        _serializer.Encode(writer, _getter(source));
    }
    
    /// <inheritdoc />
    public void Decode(TEntity target, IDataBufferReader reader, bool existing = false)
    {
        TProperty current = _serializer.Decode(reader);

        if (_key && existing)
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
    
    #endregion
}