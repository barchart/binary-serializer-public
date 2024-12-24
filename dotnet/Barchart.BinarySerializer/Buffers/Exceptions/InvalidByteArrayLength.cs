namespace Barchart.BinarySerializer.Buffers.Exceptions;

/// <summary>
///     Thrown when the length of a byte array is invalid.
/// </summary>
public class InvalidByteArrayLength : ArgumentOutOfRangeException
{
    #region Constructor(s)
    
    public InvalidByteArrayLength(int length) : base( $"The byte array length must be positive. The length was {length}.")
    {
        
    }
    
    #endregion
}