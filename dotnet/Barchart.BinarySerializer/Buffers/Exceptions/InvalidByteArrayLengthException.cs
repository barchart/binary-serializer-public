namespace Barchart.BinarySerializer.Buffers.Exceptions;

/// <summary>
///     Thrown when the length of a byte array is invalid.
/// </summary>
public class InvalidByteArrayLengthException : ArgumentOutOfRangeException
{
    #region Constructor(s)
    
    /// <summary>
    ///     Creates a new <see cref="InvalidByteArrayLengthException"/> instance.
    /// </summary>
    /// <param name="length">
    ///     The length of the byte array.
    /// </param>
    public InvalidByteArrayLengthException(int length) : base( $"The byte array length must be positive. The length was {length}.")
    {
        
    }
    
    #endregion
}