namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when an attempt is made alter the key of an
///     existing entity (during deserialization). 
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
    ///     The name of the key which would have been altered
    ///     during a deserialization operation.
    /// </summary>
    public string KeyName => _keyName;

    #endregion
}