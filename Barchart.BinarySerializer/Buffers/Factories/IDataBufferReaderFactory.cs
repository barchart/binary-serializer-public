namespace Barchart.BinarySerializer.Buffers.Factories;

public interface IDataBufferReaderFactory
{
    #region Methods

    IDataBufferReader Make(byte[] byteArray);

    #endregion
}