#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Headers;

/// <summary>
///     Represents the binary header serializer responsible for encoding and decoding
///     headers in the binary data. A header typically contains metadata such as an entity ID
///     and snapshot information that is used to interpret the serialized data.
/// </summary>
public class BinaryHeaderSerializer
{
    #region Constants
    
    private const byte SNAPSHOT_FLAG = 128;
    
    private const byte MAX_ENTITY_ID = 15;
    
    #endregion
    
    #region Properties

    /// <summary>
    ///     The singleton instance of the <see cref="BinaryHeaderSerializer"/>.
    /// </summary>
    public static BinaryHeaderSerializer Instance { get; } = new();

    #endregion
    
    #region Constructor(s)

    private BinaryHeaderSerializer()
    {
        
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    ///     Serializes a header into the provided data buffer writer.
    /// </summary>
    /// <param name="writer">
    ///     The data buffer writer to which the header will be written.
    /// </param>
    /// <param name="entityId">
    ///     The entity ID to be included in the header. This ID helps identify the type of entity
    ///     the data represents.
    /// </param>
    /// <param name="snapshot">
    ///     A boolean value indicating whether the data represents a snapshot. If true, the
    ///     snapshot flag will be set in the header.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the entityId argument exceeds the maximum value of 15.
    /// </exception>
    public void Encode(IDataBufferWriter writer, byte entityId, bool snapshot)
    {
        if (entityId > MAX_ENTITY_ID)
        {
            throw new ArgumentOutOfRangeException(nameof(entityId), $"The entityId argument cannot exceed {MAX_ENTITY_ID} because the header serializer uses exactly four bits for entityId value.");
        }
        
        byte combined = entityId;

        if (snapshot)
        {
            combined = (byte)(combined ^ SNAPSHOT_FLAG);
        }

        writer.WriteByte(combined);
    }
    
    /// <summary>
    ///     Deserializes a header from the provided data buffer reader.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader from which the header will be read.
    /// </param>
    /// <returns>
    ///     An <see cref="Header"/> instance representing the decoded header. The header includes
    ///     information such as the entity ID and whether the data is a snapshot.
    /// </returns>
    /// <exception cref="InvalidHeaderException">
    ///    Thrown when the entityId value exceeds the maximum value of 15.
    /// </exception>
    public Header Decode(IDataBufferReader reader)
    {
        byte combined = reader.ReadByte();
        
        bool snapshot = (combined & SNAPSHOT_FLAG) == SNAPSHOT_FLAG;

        byte entityId = combined;
        
        if (snapshot)
        {
            entityId = (byte)(entityId ^ SNAPSHOT_FLAG);
        }

        if (entityId > MAX_ENTITY_ID)
        {
            throw new InvalidHeaderException(MAX_ENTITY_ID);
        }
        
        return new Header(entityId, snapshot);
    }
    
    #endregion
}