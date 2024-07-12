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

    [Fact]
    public void Make_WithDefaultConstructor_UsesDefaultByteArrayLength()
    {
        var writer = _factory.Make(new object());

        Assert.IsAssignableFrom<IDataBufferWriter>(writer);
    }

    [Theory]
    [InlineData(1024)]
    [InlineData(2048)]
    [InlineData(4096)]
    public void Make_WithCustomByteArrayLength_UsesSpecifiedLength(int byteArrayLength)
    {
        var customFactory = new DataBufferWriterFactory(byteArrayLength);
        var writer = customFactory.Make(new object());

        Assert.IsAssignableFrom<IDataBufferWriter>(writer);
    }

    [Fact]
    public void Make_CalledMultipleTimes_ReusesThreadStaticByteArray()
    {
        var writer1 = _factory.Make(new object());
        var writer2 = _factory.Make(new object());

        Assert.IsAssignableFrom<IDataBufferWriter>(writer1);
        Assert.IsAssignableFrom<IDataBufferWriter>(writer2);
    }
}