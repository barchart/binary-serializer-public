namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when there is a mismatch between the expected and actual header during deserialization.
/// </summary>
public class HeaderMismatchException : InvalidOperationException
{
    #region Constructor(s)
    
    public HeaderMismatchException(byte entityId, byte expectedEntityId) : base($"The header entity ID ({entityId}) does not match the expected entity ID ({expectedEntityId}).")
    {
        
    }
    
    #endregion
}