namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when attempting to access a key property (of a schema)
///     that does not exist.
/// </summary>
public class KeyUndefinedException : InvalidOperationException
{
    #region Constructor(s)

    /// <summary>
    ///     Creates a new instance of the <see cref="KeyUndefinedException"/> class.
    /// </summary>
    /// <param name="entityType">
    ///     The type of the entity.
    /// </param>
    /// <param name="keyName">
    ///     The name of the key.
    /// </param>
    /// <param name="keyType">
    ///     The type of the key.
    /// </param>
    public KeyUndefinedException(Type entityType, string keyName, Type keyType) : base($"The schema for [ {entityType.Name} ] does not contain a key property with the specified name and type [ {keyName} ] [ {keyType.Name} ].")
    {
        
    }
    
    #endregion
}