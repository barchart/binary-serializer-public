namespace Barchart.BinarySerializer.Buffers.Factories;

/// <summary>
///     An implementation of <see cref="IDataBufferWriterFactory" /> that uses
///     thead-static byte arrays as the underlying binary data storage for
///     each <see cref="IDataBufferWriter" />.
/// </summary>
public class DataBufferWriterFactory : IDataBufferWriterFactory
{
    #region Constants

    private const int DEFAULT_BYTE_ARRAY_LENGTH = 512 * 1024;
    
    #endregion
    
    #region Fields
    
    [ThreadStatic]
    private static byte[]? _byteArray;

    private int _byteArrayLength;
    
    #endregion
    
    #region Constructor(s)

    public DataBufferWriterFactory() : this(DEFAULT_BYTE_ARRAY_LENGTH)
    {
        
    }
    
    public DataBufferWriterFactory(int byteArrayLength)
    {
        _byteArrayLength = byteArrayLength;
    }
    
    #endregion

    #region Methods

    /// <inheritdoc />
    public IDataBufferWriter Make<TEntity>(TEntity source)
    {
        if (_byteArray == null)
        {
            _byteArray = new byte[_byteArrayLength];
        }

        return new DataBufferWriter(_byteArray);
    }
    
    #endregion
}