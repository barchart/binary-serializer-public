#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

public interface ISchema<TEntity> where TEntity: new()
{
    byte[] Serialize(IDataBufferWriter writer, TEntity source);

    byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous);

    TEntity Deserialize(IDataBufferReader reader);
    
    TEntity Deserialize(IDataBufferReader reader, TEntity target);
}