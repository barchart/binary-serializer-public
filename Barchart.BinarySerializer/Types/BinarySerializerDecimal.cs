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

    private readonly BinarySerializerInt _binarySerializerInt;

    #endregion

    #region Constructors

    public BinarySerializerDecimal()
    {
        _binarySerializerInt = new BinarySerializerInt();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, decimal value)
    {
        int[] components = Decimal.GetBits(value);

        _binarySerializerInt.Encode(buffer, components[0]);
        _binarySerializerInt.Encode(buffer, components[1]);
        _binarySerializerInt.Encode(buffer, components[2]);
        _binarySerializerInt.Encode(buffer, components[3]);
    }

    /// <inheritdoc />
    public decimal Decode(IDataBufferReader buffer)
    {
        int[] components =
        {
            _binarySerializerInt.Decode(buffer),
            _binarySerializerInt.Decode(buffer),
            _binarySerializerInt.Decode(buffer),
            _binarySerializerInt.Decode(buffer)
        };

        return new decimal(components);
    }

    /// <inheritdoc />
    public bool GetEquals(decimal a, decimal b)
    {
        return a.Equals(b);
    }

    #endregion
}