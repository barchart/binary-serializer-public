namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when attempting to access a key property (of a schema)
///     that does not exist.
/// </summary>
public class KeyUndefinedException : InvalidOperationException
{
    #region Properties

    /// <summary>
    ///     The type of the entity.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    ///     The type of the key.
    /// </summary>
    public Type KeyType { get; }

    /// <summary>
    ///     The name of the key.
    /// </summary>
    public string KeyName { get; }

    #endregion
    
    #region Constructor(s)

    public KeyUndefinedException(Type entityType, string keyName, Type keyType) : base($"The schema for [ {entityType.Name} ] does not contain a key property with the specified name and type [ {keyName} ] [ {keyType.Name} ].")
    {
        EntityType = entityType;
        
        KeyName = keyName;
        KeyType = keyType;
    }
    
    #endregion
}