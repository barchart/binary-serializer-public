namespace Barchart.BinarySerializer.Buffers.Factories;

/// <summary>
///     An implementation of <see cref="IDataBufferReaderFactory" /> that uses byte arrays 
///     as the underlying binary data storage foreach <see cref="IDataBufferReader" />.
/// </summary>
public class DataBufferReaderFactory : IDataBufferReaderFactory
{
    #region Methods

    /// <inheritdoc />
    public IDataBufferReader Make(byte[] byteArray)
    {
        return new DataBufferReader(byteArray);
    }
    
    #endregion
}