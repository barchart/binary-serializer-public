#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

public class BinarySerializerSbyte : IBinaryTypeSerializer<sbyte>
{
    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, sbyte value)
    {
        buffer.WriteByte((byte)value);
    }

    /// <inheritdoc />
    public sbyte Decode(IDataBufferReader buffer)
    {
        return (sbyte)buffer.ReadByte();
    }

    /// <inheritdoc />
    public bool GetEquals(sbyte a, sbyte b)
    {
        return a.Equals(b);
    }

    #endregion
}