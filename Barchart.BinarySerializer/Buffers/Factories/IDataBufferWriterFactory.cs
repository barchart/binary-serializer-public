namespace Barchart.BinarySerializer.Buffers.Factories;

/// <summary>
///     A strategy for creating <see cref="IDataBufferWriter" /> instances.
/// </summary>
public interface IDataBufferWriterFactory
{
    /// <summary>
    ///     Creates a <see cref="IDataBufferWriter" /> instance.
    /// </summary>
    /// <param name="entity">
    ///     The entity being serialized.
    /// </param>
    IDataBufferWriter Make<TEntity>(TEntity entity);
}