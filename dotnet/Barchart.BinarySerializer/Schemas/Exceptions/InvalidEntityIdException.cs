namespace Barchart.BinarySerializer.Schemas.Exceptions;

/// <summary>
///     Thrown when an entity ID is invalid (i.e. [ 0 ]).
/// </summary>
public class InvalidEntityIdException : ArgumentException
{
    #region Constructor(s)
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidEntityIdException"/> class.
    /// </summary>
    public InvalidEntityIdException() : base("Entity ID cannot be [ 0 ].")
    {
        
    }
    
    #endregion
}