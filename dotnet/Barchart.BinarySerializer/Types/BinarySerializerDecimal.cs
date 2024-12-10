#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) decimal values to (and from) a binary data source.
/// </summary>
public class BinarySerializerDecimal : IBinaryTypeSerializer<decimal>
{
    #region Fields

    private readonly BinarySerializerInt _binarySerializerInt = new();

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, decimal value)
    {
        int[] components = decimal.GetBits(value);

        _binarySerializerInt.Encode(writer, components[0]);
        _binarySerializerInt.Encode(writer, components[1]);
        _binarySerializerInt.Encode(writer, components[2]);
        _binarySerializerInt.Encode(writer, components[3]);
    }

    /// <inheritdoc />
    public decimal Decode(IDataBufferReader reader)
    {
        int[] components =
        [
            _binarySerializerInt.Decode(reader),
            _binarySerializerInt.Decode(reader),
            _binarySerializerInt.Decode(reader),
            _binarySerializerInt.Decode(reader)
        ];

        return new decimal(components);
    }

    /// <inheritdoc />
    public bool GetEquals(decimal a, decimal b)
    {
        return a.Equals(b);
    }

    #endregion
}