#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) bytes to (and from) a binary data source.
/// </summary>
public class BinarySerializerByte : IBinaryTypeSerializer<byte>
{
    #region Methods
    
    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, byte value)
    {
        writer.WriteByte(value);
    }

    /// <inheritdoc />
    public byte Decode(IDataBufferReader reader)
    {
        return reader.ReadByte();
    }
    
    /// <inheritdoc />
    public bool GetEquals(byte a, byte b)
    {
        return a.Equals(b);
    }
    
    #endregion
}