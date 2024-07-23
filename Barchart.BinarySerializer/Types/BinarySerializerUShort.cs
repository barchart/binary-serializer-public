#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) ushort values to (and from) a binary data source.
/// </summary>
public class BinarySerializerUShort : IBinaryTypeSerializer<ushort>
{
    #region Constants
        
    private const int ENCODED_LENGTH_IN_BYTES = sizeof(ushort);
        
    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, ushort value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public ushort Decode(IDataBufferReader reader)
    {
        return BitConverter.ToUInt16(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }
        
    /// <inheritdoc />
    public bool GetEquals(ushort a, ushort b)
    {
        return a.Equals(b);
    }

    #endregion
}