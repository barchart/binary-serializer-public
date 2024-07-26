namespace Barchart.BinarySerializer.Buffers.Factories;

public class DataBufferReaderFactory : IDataBufferReaderFactory
{
    #region Methods

    public IDataBufferReader Make(byte[] byteArray)
    {
        return new DataBufferReader(byteArray);
    }
    
    #endregion
}