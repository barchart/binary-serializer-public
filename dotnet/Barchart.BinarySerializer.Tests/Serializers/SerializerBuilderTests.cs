#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Serializers;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers;

public class SerializerBuilderTests
{

    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly SerializerBuilder<TestEntity> _serializerBuilder;
    
    #endregion

    #region Constructor(s)

    public SerializerBuilderTests(ITestOutputHelper testOutputHelper)
    {
        const byte entityId = 1;

        _testOutputHelper = testOutputHelper;
            
        _serializerBuilder = new SerializerBuilder<TestEntity>(entityId);
    }

    #endregion

    #region Test Methods (WithSchemaFactory)
        
    [Fact]
    public void WithSchemaFactory_WhenCalled_SetsSchemaFactory()
    {
        SchemaFactory schemaFactory = new();

        _serializerBuilder.WithSchemaFactory(schemaFactory);
        Serializer<TestEntity> serializer = _serializerBuilder.Build();

        Assert.NotNull(serializer);
    }

    [Fact]
    public void WithSchemaFactory_WhenCalledWithTypeSerializerFactory_SetsSchemaFactory()
    {
        BinaryTypeSerializerFactory typeSerializerFactory = new();

        _serializerBuilder.WithSchemaFactory(typeSerializerFactory);
        Serializer<TestEntity> serializer = _serializerBuilder.Build();

        Assert.NotNull(serializer);
    }
        
    #endregion

    #region Test Methods (WithDataBufferReaderFactory)

    [Fact]
    public void WithDataBufferReaderFactory_WhenCalled_SetsDataBufferReaderFactory()
    {
        DataBufferReaderFactory dataBufferReaderFactory = new();

        _serializerBuilder.WithDataBufferReaderFactory(dataBufferReaderFactory);
        Serializer<TestEntity> serializer = _serializerBuilder.Build();

        Assert.NotNull(serializer);
    }
        
    #endregion

    #region Test Methods (WithDataBufferWriterFactory)

    [Fact]
    public void WithDataBufferWriterFactory_WhenCalled_SetsDataBufferWriterFactory()
    {
        DataBufferWriterFactory dataBufferWriterFactory = new();

        _serializerBuilder.WithDataBufferWriterFactory(dataBufferWriterFactory);
        Serializer<TestEntity> serializer = _serializerBuilder.Build();

        Assert.NotNull(serializer);
    }
        
    #endregion
        
    #region Test Methods (Build)

    [Fact]
    public void Build_WhenCalled_ReturnsInstanceOfSerializer()
    {
        Serializer<TestEntity> serializer = _serializerBuilder.Build();

        Assert.NotNull(serializer);
        Assert.IsType<Serializer<TestEntity>>(serializer);
    }

    #endregion

    #region Test Methods (ForType)

    [Fact]
    public void ForType_WhenCalled_ReturnsNewSerializerBuilderInstance()
    {
        SerializerBuilder<TestEntity> builder = SerializerBuilder<TestEntity>.ForType();

        Assert.NotNull(builder);
        Assert.IsType<SerializerBuilder<TestEntity>>(builder);
    }

    #endregion
        
    #region Nested Types

    private class TestEntity
    {
        [Serialize(true)]
        public string KeyProperty { get; set; } = "";

        [Serialize]
        public string ValueProperty { get; set; } = "";
    }

    #endregion
}