namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when there is a mismatch between the expected and actual header during deserialization.
/// </summary>
public class HeaderMismatchException : InvalidOperationException
{
    #region Constructor(s)
    
    public HeaderMismatchException(string message) : base(message)
    {
        
    }
    
    #endregion
}