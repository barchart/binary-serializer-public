#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

public class BinarySerializerInt : IBinaryTypeSerializer<int>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(int);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, int value)
    {
        buffer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public int Decode(IDataBufferReader buffer)
    {
        return BitConverter.ToInt32(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(int a, int b)
    {
        return a.Equals(b);
    }

    #endregion
}