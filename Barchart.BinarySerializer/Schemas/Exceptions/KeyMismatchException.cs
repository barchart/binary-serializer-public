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
    
    public KeyMismatchException(string keyName) : base($"An attempt was made to alter the {keyName} during deserialization")
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