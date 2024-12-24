namespace Barchart.BinarySerializer.Types.Exceptions;

/// <summary>
///     Thrown when the length of a string is invalid.
/// </summary>
public class InvalidStringLengthException : ArgumentException
{
    #region Constructor(s)
    
    public InvalidStringLengthException(int length, int maximumStringLengthInBytes) : base($"Unable to serialize string. Serialized string would require {length} bytes; however, the maximum size of a serialized string is {maximumStringLengthInBytes}")
    {
        
    }
    
    #endregion
}