#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

public interface ISchema<TEntity> where TEntity: new()
{
    byte[] Serialize(TEntity source, IDataBufferWriter writer);

    void Deserialize(TEntity target, IDataBufferReader reader);
}