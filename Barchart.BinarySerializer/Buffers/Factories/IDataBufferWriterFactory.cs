namespace Barchart.BinarySerializer.Buffers.Factories;

/// <summary>
///     A strategy for creating <see cref="IDataBufferWriter" /> instances.
/// </summary>
public interface IDataBufferWriterFactory
{
    #region Methods

    /// <summary>
    ///     Creates an <see cref="IDataBufferWriter" /> instance.
    /// </summary>
    /// <param name="entity">
    ///     The entity being serialized.
    /// </param>
    IDataBufferWriter Make<TEntity>(TEntity entity);

    #endregion
}