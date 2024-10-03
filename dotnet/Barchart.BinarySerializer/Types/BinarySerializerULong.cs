#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) ulong values to (and from) a binary data source.
/// </summary>
public class BinarySerializerULong : IBinaryTypeSerializer<ulong>
{
    #region Constants
        
    private const int ENCODED_LENGTH_IN_BYTES = sizeof(ulong);
        
    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, ulong value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public ulong Decode(IDataBufferReader reader)
    {
        return BitConverter.ToUInt64(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }
        
    /// <inheritdoc />
    public bool GetEquals(ulong a, ulong b)
    {
        return a.Equals(b);
    }

    #endregion
}