namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when attempting to access a key property (of a schema)
///     that does not exist.
/// </summary>
public class KeyUndefinedException : InvalidOperationException
{
    #region Fields

    private readonly Type _entityType;
    
    private readonly string _keyName;
    private readonly Type _keyType;

    #endregion
    
    #region Constructor(s)

    public KeyUndefinedException(Type entityType, string keyName, Type keyType) : base($"The schema for [ {entityType.Name} ] does not contain a key property with the specified name and type [ {keyName} ] [ {keyType.Name} ].")
    {
        _entityType = entityType;
        
        _keyName = keyName;
        _keyType = keyType;
    }
    
    #endregion
    
    #region Properties

    /// <summary>
    ///     The type of the entity.
    /// </summary>
    public Type EntityType => _entityType;
    
    /// <summary>
    ///     The type of the key.
    /// </summary>
    public Type KeyType => _keyType;
    
    /// <summary>
    ///     The name of the key.
    /// </summary>
    public string KeyName => _keyName;

    #endregion
}