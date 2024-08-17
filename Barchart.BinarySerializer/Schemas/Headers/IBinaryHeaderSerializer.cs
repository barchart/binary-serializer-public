#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

public interface IBinaryHeaderSerializer
{
    
    public void Encode(IDataBufferWriter writer, byte entityId, bool snapshot);
    
    public IHeader Decode(IDataBufferReader reader);
}