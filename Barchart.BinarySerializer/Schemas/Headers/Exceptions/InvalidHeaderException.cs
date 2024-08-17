namespace Barchart.BinarySerializer.Schemas.Exceptions;

public class InvalidHeaderException : Exception
{
    #region Constructor(s)
    
    public InvalidHeaderException(string message) : base(message)
    {
    }
    
    #endregion
}