#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) boolean values to (and from) a binary data source.
/// </summary>
public class BinarySerializerBool : IBinaryTypeSerializer<bool>
{
    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, bool value)
    {
        buffer.WriteBit(value);
    }

    /// <inheritdoc />
    public bool Decode(IDataBufferReader buffer)
    {
        return buffer.ReadBit();
    }

    /// <inheritdoc />
    public bool GetEquals(bool a, bool b)
    {
        return a.Equals(b);
    }

    #endregion
}