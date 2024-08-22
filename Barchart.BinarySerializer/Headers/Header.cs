namespace Barchart.BinarySerializer.Headers;

public struct Header
{
    #region Properties
    
    /// <summary>
    ///     An identifier for the entity that was serialized.
    /// </summary>
    public byte EntityId { get; }
    
    /// <summary>
    ///     True if the message is a "snapshot" and contains all
    ///     properties of the entity. False if the message is
    ///     a "delta" and omits properties that have not changed.
    /// </summary>
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