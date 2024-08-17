namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when a serialized header cannot be parsed.
/// </summary>
public class InvalidHeaderException : Exception
{
    #region Constructor(s)
    
    public InvalidHeaderException(string message) : base(message)
    {
    }
    
    #endregion
}