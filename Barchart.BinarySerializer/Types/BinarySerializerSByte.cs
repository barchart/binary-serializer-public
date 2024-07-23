#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) sbyte values to (and from) a binary data source.
/// </summary>
public class BinarySerializerSByte : IBinaryTypeSerializer<sbyte>
{
    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, sbyte value)
    {
        writer.WriteByte((byte)value);
    }

    /// <inheritdoc />
    public sbyte Decode(IDataBufferReader reader)
    {
        return (sbyte)reader.ReadByte();
    }

    /// <inheritdoc />
    public bool GetEquals(sbyte a, sbyte b)
    {
        return a.Equals(b);
    }

    #endregion
}