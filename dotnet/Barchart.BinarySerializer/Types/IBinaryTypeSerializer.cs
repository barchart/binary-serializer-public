﻿#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types;

/// <summary>
///     A marker interface for <see cref="IBinaryTypeSerializer{T}"/>.
/// </summary>
public interface IBinaryTypeSerializer
{
    
}

/// <summary>
///     Writes (and reads) values of type <typeparamref name="T" /> to (and from)
///     a binary data source.
/// </summary>
/// <typeparam name="T">
///     The type to serialize to binary (and deserialize from binary).
/// </typeparam>
public interface IBinaryTypeSerializer<T> : IBinaryTypeSerializer
{
    #region Methods

    /// <summary>
    ///     Writes a value to a binary data source.
    /// </summary>
    /// <param name="writer">
    ///     The binary data source.
    /// </param>
    /// <param name="value">
    ///     The value to write.
    /// </param>
    public void Encode(IDataBufferWriter writer, T value);

    /// <summary>
    ///     Reads a value from a binary data source.
    /// </summary>
    /// <param name="reader">
    ///     The binary data source.
    /// </param>
    /// <returns>
    ///     The value.
    /// </returns>
    public T Decode(IDataBufferReader reader);

    /// <summary>
    ///     Indicates whether two values are equal. Presence of this
    ///     method prevents the need for boxing since the type parameter
    ///     is not constrained to <see cref="IEquatable{T}" />.
    /// </summary>
    /// <param name="a">
    ///     The first value.
    /// </param>
    /// <param name="b">
    ///     The second value.
    /// </param>
    /// <returns>
    ///     True if the two values are equal; otherwise false.
    /// </returns>
    public bool GetEquals(T a, T b);

    #endregion
}