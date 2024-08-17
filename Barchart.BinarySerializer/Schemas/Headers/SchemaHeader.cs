namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     The default implementation of the <see cref="ISchemaHeader" />.
/// </summary>
public struct SchemaHeader : ISchemaHeader
{
    #region Fields
    
    private readonly byte _entityId;
    private readonly bool _snapshot;
    
    #endregion
    
    #region Constructor(s)
    
    public SchemaHeader(byte entityId, bool snapshot)
    {
        _entityId = entityId;
        _snapshot = snapshot;
    }
    
    #endregion
    
    #region Properties
    
    /// <inheritdoc />
    public byte EntityId => _entityId;
    
    /// <inheritdoc />
    public bool Snapshot => _snapshot;
    
    #endregion
}