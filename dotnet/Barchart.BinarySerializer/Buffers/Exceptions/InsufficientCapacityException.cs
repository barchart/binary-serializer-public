namespace Barchart.BinarySerializer.Buffers.Exceptions;

/// <summary>
///     Thrown when attempting to change an entity's key value
///     (during deserialization) or when attempting to compare
///     two entities which have different key values (during
///     serialization).
/// </summary>
public class InsufficientCapacityException : InvalidOperationException
{
    #region Constructor(s)
    
    public InsufficientCapacityException(bool writing) : base(writing ? "Unable to write to [IDataBufferWriter], remaining capacity would be exceeded." : "Unable to read from [IDataBufferReader], remaining capacity would be exceeded.")
    {

    }
    
    #endregion
}