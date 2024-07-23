namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when attempting to change an entity's key value
///     (during deserialization) or when attempting to compare
///     two entities which have different key values (during
///     serialization).
/// </summary>
public class KeyMismatchException : InvalidOperationException
{
    #region Fields

    private readonly Type _entityType;

    private readonly string _keyName;

    #endregion

    #region Properties

    /// <summary>
    ///     The type of the entity.
    /// </summary>
    public Type EntityType => _entityType;
    
    /// <summary>
    ///     The name of the key.
    /// </summary>
    public string KeyName => _keyName;

    #endregion
    
    #region Constructor(s)
    
    public KeyMismatchException(Type entityType, string keyName, bool serializing) : base(serializing ? $"An attempt was made to serialize the difference between two entities with different key values [ {keyName} ]." : $"An attempt was made to alter the a key property during deserialization [ {keyName} ].")
    {
        _entityType = entityType;
        
        _keyName = keyName;
    }
    
    #endregion
}