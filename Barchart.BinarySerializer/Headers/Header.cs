namespace Barchart.BinarySerializer.Headers;

public struct Header
{
    #region Properties
    
    public byte EntityId { get; }
    
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