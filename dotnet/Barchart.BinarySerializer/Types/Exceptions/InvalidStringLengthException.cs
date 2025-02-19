namespace Barchart.BinarySerializer.Types.Exceptions;

/// <summary>
///     Thrown when the length of a string is invalid.
/// </summary>
public class InvalidStringLengthException : ArgumentException
{
    #region Constructor(s)
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidStringLengthException"/> class.
    /// </summary>
    /// <param name="length">
    ///     The length of the string.
    /// </param>
    /// <param name="maximumStringLengthInBytes">
    ///     The maximum size of a serialized string.
    /// </param>
    public InvalidStringLengthException(int length, int maximumStringLengthInBytes) : base($"Unable to serialize string. Serialized string would require {length} bytes; however, the maximum size of a serialized string is {maximumStringLengthInBytes}")
    {
        
    }
    
    #endregion
}