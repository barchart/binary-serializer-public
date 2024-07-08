#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

public class BinarySerializerUInt : IBinaryTypeSerializer<uint>
{
    #region Constants
        
    private const int ENCODED_LENGTH_IN_BYTES = sizeof(uint);
        
    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, uint value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public uint Decode(IDataBufferReader reader)
    {
        return BitConverter.ToUInt32(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(uint a, uint b)
    {
        return a.Equals(b);
    }
        
    #endregion
}