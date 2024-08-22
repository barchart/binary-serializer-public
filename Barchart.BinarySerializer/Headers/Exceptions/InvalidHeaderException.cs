namespace Barchart.BinarySerializer.Headers.Exceptions;

/// <summary>
///     Thrown when a serialized header cannot be parsed.
/// </summary>
public class InvalidHeaderException : InvalidOperationException
{
    #region Constructor(s)
    
    public InvalidHeaderException(string message) : base(message)
    {
    }
    
    #endregion
}