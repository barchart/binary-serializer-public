#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Headers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     The default implementation of <see cref="IBinaryHeaderSerializer" />.
/// </summary>
public class BinaryHeaderSerializer : IBinaryHeaderSerializer
{
    #region Fields
    
    private const byte SNAPSHOT_FLAG = 128;
    
    #endregion

    #region Methods
    
    /// <inheritdoc />
    public virtual void Encode(IDataBufferWriter writer, byte entityId, bool snapshot)
    {
        if (entityId > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(entityId), "The entityId argument cannot exceed 15 because the header serializer is configured to use four bits for entityId value.");
        }
        
        byte combined = entityId;

        if (snapshot)
        {
            combined = (byte)(combined ^ SNAPSHOT_FLAG);
        }

        writer.WriteByte(combined);
    }

    /// <inheritdoc />
    public virtual IHeader Decode(IDataBufferReader reader)
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
    
    #endregion
}