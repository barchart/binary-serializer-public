#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) long values to (and from) a binary data source.
/// </summary>
public class BinarySerializerLong : IBinaryTypeSerializer<long>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(long);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, long value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public long Decode(IDataBufferReader reader)
    {
        return BitConverter.ToInt64(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(long a, long b)
    {
        return a.Equals(b);
    }

    #endregion
}