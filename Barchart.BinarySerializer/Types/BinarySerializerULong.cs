#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

public class BinarySerializerULong : IBinaryTypeSerializer<ulong>
{
    #region Constants
        
    private const int ENCODED_LENGTH_IN_BYTES = sizeof(ulong);
        
    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, ulong value)
    {
        buffer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public ulong Decode(IDataBufferReader buffer)
    {
        return BitConverter.ToUInt64(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }
        
    /// <inheritdoc />
    public bool GetEquals(ulong a, ulong b)
    {
        return a.Equals(b);
    }

    #endregion
}