#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) int values to (and from) a binary data source.
/// </summary>
public class BinarySerializerInt : IBinaryTypeSerializer<int>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(int);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, int value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public int Decode(IDataBufferReader reader)
    {
        return BitConverter.ToInt32(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(int a, int b)
    {
        return a.Equals(b);
    }

    #endregion
}