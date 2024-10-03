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
    ///     Indicates if the factory can make an <see cref="IBinaryTypeSerializer{T}"/> for
    ///     the specified type.
    /// </summary>
    /// <param name="type">
    ///     The type to test.
    /// </param>
    /// <returns>
    ///     True, if the factory can make an <see cref="IBinaryTypeSerializer{T}"/> for
    ///     the specified type.
    /// </returns>
    bool Supports(Type type);
    
    /// <summary>
    ///     Indicates if the factory can make an <see cref="IBinaryTypeSerializer{T}"/> for
    ///     the specified type.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to test.
    /// </typeparam>
    /// <returns>
    ///     True, if the factory can make an <see cref="IBinaryTypeSerializer{T}"/> for
    ///     the specified type.
    /// </returns>
    bool Supports<T>();
    
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