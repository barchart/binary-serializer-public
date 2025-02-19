namespace Barchart.BinarySerializer.Headers.Exceptions;

/// <summary>
///     Thrown when a serialized header cannot be parsed.
/// </summary>
public class InvalidHeaderException : InvalidOperationException
{
    #region Constructor(s)
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
    /// </summary>
    /// <param name="maxEntityId">
    ///     The maximum entityId value that can be used.
    /// </param>
    public InvalidHeaderException(byte maxEntityId) : base($"The entityId cannot exceed {maxEntityId} because the header serializer uses exactly four bits for entityId value.")
    {
        
    }
    
    #endregion
}