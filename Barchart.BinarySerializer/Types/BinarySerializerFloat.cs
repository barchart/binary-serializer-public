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
    public void Encode(IDataBufferWriter writer, float value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public float Decode(IDataBufferReader reader)
    {
        return BitConverter.ToSingle(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(float a, float b)
    {
        return a.Equals(b);
    }

    #endregion
}