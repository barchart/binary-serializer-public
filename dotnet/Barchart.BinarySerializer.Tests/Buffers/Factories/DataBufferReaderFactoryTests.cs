#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Buffers.Factories;

public class DataBufferReaderFactoryTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly DataBufferReaderFactory _factory;

    #endregion

    #region Constructor(s)

    public DataBufferReaderFactoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _factory = new DataBufferReaderFactory();
    }

    #endregion

    #region Test Methods (Make<T>)
    
    [Fact]
    public void Make_WithDefaultFactory_ReturnsIDataBufferReaderInstance()
    {
        byte[] byteArray = new byte[10];
        IDataBufferReader reader = _factory.Make(byteArray);

        Assert.IsAssignableFrom<IDataBufferReader>(reader);
    }
    
    #endregion
}