namespace Barchart.BinarySerializer.Buffers.Factories;

/// <summary>
///     A strategy for creating <see cref="IDataBufferReader" /> instances.
/// </summary>
public interface IDataBufferReaderFactory
{
    #region Methods

    /// <summary>
    ///     Creates an <see cref="IDataBufferReader" /> instance.
    /// </summary>
    IDataBufferReader Make(byte[] byteArray);

    #endregion
}