#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     The default implementation of <see cref="IHeaderSerializer" />.
/// </summary>
public class HeaderSerializer : IHeaderSerializer
{
    #region Fields
    
    private const byte SNAPSHOT_FLAG = 128;
    
    #endregion
    
    #region Constructor(s)
    
    public HeaderSerializer()
    {

    }
    
    #endregion
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, byte entityId, bool snapshot)
    {
        if (entityId > 15)
        {
            throw new ArgumentOutOfRangeException("entityId", "The entityId argument cannot exceed 15 because the header serializer is configured to use four bits for entityId value.");
        }
        
        byte combined = entityId;

        if (snapshot)
        {
            combined = (byte)(combined ^ SNAPSHOT_FLAG);
        }

        writer.WriteByte(combined);
    }

    /// <inheritdoc />
    public IHeader Decode(IDataBufferReader reader)
    {
        byte combined = reader.ReadByte();
        
        bool snapshot = (combined & SNAPSHOT_FLAG) == SNAPSHOT_FLAG;

        byte entityId = combined;
        
        if (snapshot)
        {
            entityId = (byte)(entityId ^ SNAPSHOT_FLAG);
        }

        if (entityId > 15)
        {
            throw new InvalidHeaderException("The entityId cannot exceed 15 because the header serializer is configured to use four bits for entityId value.");
        }
        
        return new Header(entityId, snapshot);
    }
}