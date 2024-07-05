#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     Provides binary serialization functionality for value types (that are nullable).
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

    public BinarySerializerNullable(IBinaryTypeSerializer<T> typeSerializer)
    {
        _typeSerializer = typeSerializer;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter buffer, T? value)
    {
        buffer.WriteBit(!value.HasValue);

        if (value.HasValue)
        {
            _typeSerializer.Encode(buffer, value.Value);
        }
    }

    /// <inheritdoc />
    public T? Decode(IDataBufferReader buffer)
    {
        if (buffer.ReadBit())
        {
            return null;
        }

        return _typeSerializer.Decode(buffer);
    }

    /// <inheritdoc />
    public bool GetEquals(T? a, T? b)
    {
        return (!a.HasValue && !b.HasValue) || (a.HasValue && b.HasValue && _typeSerializer.GetEquals(a.Value, b.Value));
    }

    #endregion
}