namespace Barchart.BinarySerializer.Headers.Exceptions;

/// <summary>
///     Thrown when a serialized header cannot be parsed.
/// </summary>
public class InvalidHeaderException : InvalidOperationException
{
    #region Constructor(s)
    
    public InvalidHeaderException(byte maxEntityId) : base($"The entityId cannot exceed {maxEntityId} because the header serializer uses exactly four bits for entityId value.")
    {
        
    }
    
    #endregion
}