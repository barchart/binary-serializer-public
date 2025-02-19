#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Reads (and writes) nullable value types to (and from) a binary data source.
/// </summary>
/// <typeparam name="T">
///     The value type to serialize.
/// </typeparam>
public class BinarySerializerNullable<T> : IBinaryTypeSerializer<T?> where T : struct
{
    #region Fields

    private readonly IBinaryTypeSerializer<T> _typeSerializer;

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Creates a new instance of the <see cref="BinarySerializerNullable{T}" /> class.
    /// </summary>
    /// <param name="typeSerializer">
    ///     The type serializer to use for the value type.
    /// </param>
    public BinarySerializerNullable(IBinaryTypeSerializer<T> typeSerializer)
    {
        _typeSerializer = typeSerializer;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, T? value)
    {
        Serialization.WriteNullFlag(writer, !value.HasValue);

        if (value.HasValue)
        {
            _typeSerializer.Encode(writer, value.Value);
        }
    }

    /// <inheritdoc />
    public T? Decode(IDataBufferReader reader)
    {
        if (Serialization.ReadNullFlag(reader))
        {
            return null;
        }

        return _typeSerializer.Decode(reader);
    }

    /// <inheritdoc />
    public bool GetEquals(T? a, T? b)
    {
        return (!a.HasValue && !b.HasValue) || (a.HasValue && b.HasValue && _typeSerializer.GetEquals(a.Value, b.Value));
    }

    #endregion
}