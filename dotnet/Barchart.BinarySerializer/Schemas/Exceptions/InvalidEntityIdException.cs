namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when an entity ID is invalid (i.e. [ 0 ]).
/// </summary>
public class InvalidEntityIdException : ArgumentException
{
    #region Constructor(s)
    
    public InvalidEntityIdException() : base("Entity ID cannot be [ 0 ].")
    {
        
    }
    
    #endregion
}