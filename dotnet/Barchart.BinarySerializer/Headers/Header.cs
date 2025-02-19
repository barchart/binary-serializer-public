namespace Barchart.BinarySerializer.Headers;

/// <summary>
///     Single byte representing the header of the entity.
/// </summary>
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
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Header"/> struct.
    /// </summary>
    /// <param name="entityId">
    ///     An identifier for the entity that was serialized.
    /// </param>
    /// <param name="snapshot">
    ///     True if the message is a "snapshot" or false if the message is a "delta".
    /// </param>
    public Header(byte entityId, bool snapshot)
    {
        EntityId = entityId;
        Snapshot = snapshot;
    }
    
    #endregion
}