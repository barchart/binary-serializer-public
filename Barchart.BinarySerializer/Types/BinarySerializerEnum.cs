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

    private readonly IBinaryTypeSerializer<int> _binarySerializerInt;

    #endregion

    #region Constructor(s)

    public BinarySerializerEnum(IBinaryTypeSerializer<int> binarySerializerInt)
    {
        _binarySerializerInt = binarySerializerInt;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, T value)
    {
        _binarySerializerInt.Encode(writer, Convert.ToInt32(value));
    }

    /// <inheritdoc />
    public T Decode(IDataBufferReader reader)
    {
        return (T)Enum.Parse(typeof(T), _binarySerializerInt.Decode(reader).ToString(), true);
    }

    /// <inheritdoc />
    public bool GetEquals(T a, T b)
    {
        return _binarySerializerInt.GetEquals(Convert.ToInt32(a), Convert.ToInt32(b));
    }

    #endregion
}