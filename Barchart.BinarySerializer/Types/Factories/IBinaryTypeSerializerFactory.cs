#region Using Statements

using Barchart.BinarySerializer.Types.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Types.Factories;

/// <summary>
///     Defines a factory for creating binary type serializers.
/// </summary>
public interface IBinaryTypeSerializerFactory
{
    #region Methods

    /// <summary>
    ///     Creates a binary type serializer for the specified type.
    /// </summary>
    /// <typeparam name="T">
    ///     The type for which to create a serializer.
    /// </typeparam>
    /// <exception cref="UnsupportedTypeException">
    ///     Thrown when the factory is unable to create a serializer for the specified type.
    /// </exception>
    /// <returns>
    ///     An <see cref="IBinaryTypeSerializer{T}"/> for the specified type.
    /// </returns>
    IBinaryTypeSerializer<T> Make<T>();

    #endregion
} 