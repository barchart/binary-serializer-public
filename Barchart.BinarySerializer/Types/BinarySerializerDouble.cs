#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) double values to (and from) a binary data source.
/// </summary>
public class BinarySerializerDouble : IBinaryTypeSerializer<double>
{
    #region Constants

    private const int ENCODED_LENGTH_IN_BYTES = sizeof(double);

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, double value)
    {
        writer.WriteBytes(BitConverter.GetBytes(value));
    }

    /// <inheritdoc />
    public double Decode(IDataBufferReader reader)
    {
        return BitConverter.ToDouble(reader.ReadBytes(ENCODED_LENGTH_IN_BYTES));
    }

    /// <inheritdoc />
    public bool GetEquals(double a, double b)
    {
        return a.Equals(b);
    }

    #endregion
}