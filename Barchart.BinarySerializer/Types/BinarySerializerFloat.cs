#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) float values to (and from) a binary data source.
/// </summary>
public class BinarySerializerFloat : IBinaryTypeSerializer<float>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(float);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, float value)
    {
        buffer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public float Decode(IDataBufferReader buffer)
    {
        return BitConverter.ToSingle(buffer.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(float a, float b)
    {
        return a.Equals(b);
    }

    #endregion
}