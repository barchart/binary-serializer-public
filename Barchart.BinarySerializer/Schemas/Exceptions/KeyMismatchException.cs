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

    private readonly string _keyName;

    #endregion
    
    #region Constructor(s)
    
    public KeyMismatchException(string keyName, bool serializing) : base(serializing ? $"An attempt was made to serialize the difference between two entities with different key values ({keyName})." : $"An attempt was made to alter the a key property during deserialization ({keyName}).")
    {
        _keyName = keyName;
    }
    
    #endregion
    
    #region Properties

    /// <summary>
    ///     The name of the key.
    /// </summary>
    public string KeyName => _keyName;

    #endregion
}