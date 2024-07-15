#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Buffers.Factories;

public class DataBufferWriterFactoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly DataBufferWriterFactory _factory;

    public DataBufferWriterFactoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _factory = new DataBufferWriterFactory();
    }

    #region Test Methods (Constructor)

    [Fact]
    public void Instantiate_UsingZeroByteArraySize_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataBufferWriterFactory(0));
    }
    
    [Fact]
    public void Instantiate_UsingNegativeByteArraySize_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DataBufferWriterFactory(-1));
    }
    
    #endregion
    
    #region Test Methods (Make<T>)
    
    [Fact]
    public void Make_WithDefaultFactory_ReturnsIDataBufferWriterInstance()
    {
        var writer = _factory.Make(new object());

        Assert.IsAssignableFrom<IDataBufferWriter>(writer);
    }
    
    #endregion
}