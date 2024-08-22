#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Headers;

public class BinaryHeaderSerializer
{
    #region Constants
    
    private const byte SNAPSHOT_FLAG = 128;
    
    #endregion
    
    #region Fields

    private static BinaryHeaderSerializer _instance = new BinaryHeaderSerializer();
    
    #endregion
    
    #region Constructor(s)

    private BinaryHeaderSerializer()
    {
        
    }
    
    #endregion

    #region Properties

    public static BinaryHeaderSerializer Instance => _instance;

    #endregion
    
    #region Methods
    
    public void Encode(IDataBufferWriter writer, byte entityId, bool snapshot)
    {
        if (entityId > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(entityId), "The entityId argument cannot exceed 15 because the header serializer uses exactly four bits for entityId value.");
        }
        
        byte combined = entityId;

        if (snapshot)
        {
            combined = (byte)(combined ^ SNAPSHOT_FLAG);
        }

        writer.WriteByte(combined);
    }
    
    public Header Decode(IDataBufferReader reader)
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
            throw new InvalidHeaderException("The entityId cannot exceed 15 because the header serializer uses exactly four bits for entityId value.");
        }
        
        return new Header(entityId, snapshot);
    }
    
    #endregion
}