#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Serializers;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers
{
    public class SerializerBuilderTests
    {

        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly SerializerBuilder<TestEntity> _serializerBuilder;
    
        #endregion

        #region Constructor(s)

        public SerializerBuilderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _serializerBuilder = new();
        }

        #endregion

        #region Test Methods (SerializerBuilder)

        [Fact]
        public void BuildSerializer_ReturnsInstanceOfSerializer()
        {
            Serializer<TestEntity> serializer = _serializerBuilder.Build();

            Assert.NotNull(serializer);
            Assert.IsType<Serializer<TestEntity>>(serializer);
        }

        [Fact]
        public void WithSchemaFactory_ShouldSetSchemaFactory()
        {
            SchemaFactory schemaFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithSchemaFactory(schemaFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void WithSchemaFactory_WithTypeSerializerFactory_ShouldSetSchemaFactory()
        {
            BinaryTypeSerializerFactory typeSerializerFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithSchemaFactory(typeSerializerFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void WithDataBufferReaderFactory_ShouldSetDataBufferReaderFactory()
        {
            DataBufferReaderFactory dataBufferReaderFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithDataBufferReaderFactory(dataBufferReaderFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void WithDataBufferWriterFactory_ShouldSetDataBufferWriterFactory()
        {
            DataBufferWriterFactory dataBufferWriterFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithDataBufferWriterFactory(dataBufferWriterFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void ForType_ShouldReturnNewSerializerBuilderInstance()
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
}