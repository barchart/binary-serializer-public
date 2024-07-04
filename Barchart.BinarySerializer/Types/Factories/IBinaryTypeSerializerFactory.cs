#region Using Statements

namespace Barchart.BinarySerializer.Types.Factories;

#endregion

/// <summary>
///     Defines a factory for creating binary type serializers.
/// </summary>
public interface IBinaryTypeSerializerFactory
{
    /// <summary>
    ///     Creates a binary type serializer for the specified type.
    /// </summary>
    /// <typeparam name="T">
    ///     The type for which to create a serializer.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="IBinaryTypeSerializer{T}"/> for the specified type.
    /// </returns>
    /// <remarks>
    ///     This method returns a serializer capable of encoding and decoding values of type <typeparam name="T" />
    ///     to and from a binary format. The returned serializer is typically used within a 
    ///     larger serialization schema.
    /// </remarks>
    IBinaryTypeSerializer<T> Make<T>();
} 