#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas.Headers;

public interface ISchemaHeaderSerializer<THeader> where THeader : ISchemaHeader
{
    
    public void Encode(IDataBufferWriter writer, byte entityId, bool snapshot);
    
    public THeader Decode(IDataBufferReader reader);
}