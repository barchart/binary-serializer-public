#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) enumeration values to (and from) a binary data source.
/// </summary>
/// <typeparam name="T">
///     The enumeration type.
/// </typeparam>
public class BinarySerializerEnum<T> : IBinaryTypeSerializer<T> where T : Enum
{
    #region Fields

    private readonly BinarySerializerInt _binarySerializerInt;

    #endregion

    #region Constructor(s)

    public BinarySerializerEnum(BinarySerializerInt binarySerializerInt)
    {
        _binarySerializerInt = binarySerializerInt;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, T value)
    {
        _binarySerializerInt.Encode(buffer, Convert.ToInt32(value));
    }

    /// <inheritdoc />
    public T Decode(IDataBufferReader buffer)
    {
        return (T)Enum.Parse(typeof(T), _binarySerializerInt.Decode(buffer).ToString(), true);
    }

    /// <inheritdoc />
    public bool GetEquals(T a, T b)
    {
        return _binarySerializerInt.GetEquals(Convert.ToInt32(a), Convert.ToInt32(b));
    }

    #endregion
}