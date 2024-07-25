namespace Barchart.BinarySerializer.Buffers.Factories;

public class DataBufferReaderFactory : IDataBufferReaderFactory
{
    public IDataBufferReader Make(byte[] byteArray)
    {
        return new DataBufferReader(byteArray);
    }
}