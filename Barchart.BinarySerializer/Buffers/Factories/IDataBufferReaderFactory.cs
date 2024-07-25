namespace Barchart.BinarySerializer.Buffers.Factories;

public interface IDataBufferReaderFactory
{
    IDataBufferReader Make(byte[] byteArray);
}