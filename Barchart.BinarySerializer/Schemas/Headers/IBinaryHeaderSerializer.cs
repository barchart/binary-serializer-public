#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

public interface IBinaryHeaderSerializer
{
    #region Methods

    void Encode(IDataBufferWriter writer, byte entityId, bool snapshot);
    
    IHeader Decode(IDataBufferReader reader);
    
    #endregion
}