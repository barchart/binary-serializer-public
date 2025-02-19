#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) enumeration values to (and from) a binary data source.
///     Serialization uses a single byte (with a maximum of 256 items).
/// </summary>
/// <typeparam name="T">
///     The enumeration type.
/// </typeparam>
public class BinarySerializerEnumByte<T> : IBinaryTypeSerializer<T> where T : Enum
{
    #region Fields

    private readonly IBinaryTypeSerializer<byte> _binarySerializerByte;

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Create an instance of <see cref="BinarySerializerEnumByte{T}"/>.
    /// </summary>
    /// <param name="binarySerializerByte">
    ///     The binary serializer for byte.
    /// </param>
    public BinarySerializerEnumByte(IBinaryTypeSerializer<byte> binarySerializerByte)
    {
        _binarySerializerByte = binarySerializerByte;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, T value)
    {
        _binarySerializerByte.Encode(writer, Convert.ToByte(value));
    }

    /// <inheritdoc />
    public T Decode(IDataBufferReader reader)
    {
        return (T)Enum.Parse(typeof(T), _binarySerializerByte.Decode(reader).ToString(), true);
    }

    /// <inheritdoc />
    public bool GetEquals(T a, T b)
    {
        return _binarySerializerByte.GetEquals(Convert.ToByte(a), Convert.ToByte(b));
    }

    #endregion
}