#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     Defines the binary header serializer.
/// </summary>
/// <remarks>
///     Implementations of this interface are responsible for encoding and decoding
///     headers in the binary data. A header typically contains metadata such as an entity ID
///     and snapshot information that is used to interpret the serialized data.
/// </remarks>
public interface IBinaryHeaderSerializer
{
    #region Methods
    
    /// <summary>
    ///     Serializes a Header into the provided data buffer writer.
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
    void Encode(IDataBufferWriter writer, byte entityId, bool snapshot);
    
    /// <summary>
    ///     Deserializes a Header from the provided data buffer reader.
    /// </summary>
    /// <param name="reader">
    ///     The data buffer reader from which the Header will be read.
    /// </param>
    /// <returns>
    ///     An <see cref="IHeader"/> instance representing the decoded Header. The header includes
    ///     information such as the entity ID and whether the data is a snapshot.
    /// </returns>
    IHeader Decode(IDataBufferReader reader);
    
    #endregion
}