namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     The default implementation of the <see cref="IHeader" />.
/// </summary>
public struct Header : IHeader
{
    #region Properties
    
    /// <inheritdoc />
    public byte EntityId { get; }

    /// <inheritdoc />
    public bool Snapshot { get; }

    #endregion
    
    #region Constructor(s)
    
    public Header(byte entityId, bool snapshot)
    {
        EntityId = entityId;
        Snapshot = snapshot;
    }
    
    #endregion
}