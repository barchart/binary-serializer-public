#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) short values to (and from) a binary data source.
/// </summary>
public class BinarySerializerShort : IBinaryTypeSerializer<short>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(short);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, short value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public short Decode(IDataBufferReader reader)
    {
        return BitConverter.ToInt16(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(short a, short b)
    {
        return a.Equals(b);
    }

    #endregion
}