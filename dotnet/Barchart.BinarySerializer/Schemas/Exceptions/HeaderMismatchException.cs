namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when there is a mismatch between the expected and actual header during deserialization.
/// </summary>
public class HeaderMismatchException : InvalidOperationException
{
    #region Constructor(s)
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="HeaderMismatchException"/> class.
    /// </summary>
    /// <param name="entityId">
    ///     The entity ID found in the header.
    /// </param>
    /// <param name="expectedEntityId">
    ///     The entity ID expected in the header.
    /// </param>
    public HeaderMismatchException(byte entityId, byte expectedEntityId) : base($"The header entity ID ({entityId}) does not match the expected entity ID ({expectedEntityId}).")
    {
        
    }
    
    #endregion
}