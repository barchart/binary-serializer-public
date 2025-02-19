namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when attempting to change an entity's key value
///     (during deserialization) or when attempting to compare
///     two entities which have different key values (during
///     serialization).
/// </summary>
public class KeyMismatchException : InvalidOperationException
{
    #region Constructor(s)

    /// <summary>
    ///     Creates a new <see cref="KeyMismatchException"/> instance.
    /// </summary>
    /// <param name="keyName">
    ///     The name of the key.
    /// </param>
    /// <param name="serializing">
    ///     A <see cref="bool"/> value indicating whether the exception was thrown during serialization.
    /// </param>
    public KeyMismatchException(string keyName, bool serializing) : base(serializing ? $"An attempt was made to serialize the difference between two entities with different key values [ {keyName} ]." : $"An attempt was made to alter the a key property during deserialization [ {keyName} ].")
    {
        
    }
    
    #endregion
}