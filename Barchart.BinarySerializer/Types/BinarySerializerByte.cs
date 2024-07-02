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
    public void Encode(IDataBufferWriter buffer, byte value)
    {
        buffer.WriteByte(value);
    }

    /// <inheritdoc />
    public byte Decode(IDataBufferReader buffer)
    {
        return buffer.ReadByte();
    }
    
    /// <inheritdoc />
    public bool GetEquals(byte a, byte b)
    {
        return a.Equals(b);
    }
    
    #endregion
}