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
    IDataBufferWriter Make();

    #endregion
}