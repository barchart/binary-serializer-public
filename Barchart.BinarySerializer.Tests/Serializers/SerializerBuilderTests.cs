#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers.Factories;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Schemas.Headers;
using Barchart.BinarySerializer.Serializers;
using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers
{
    public class SerializerBuilderTests
    {

        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly SerializerBuilder<TestEntity> _serializerBuilder = new();
    
        #endregion

        #region Constructor(s)

        public SerializerBuilderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region Test Methods (WithSchemaFactory)
        
        [Fact]
        public void WithSchemaFactory_WhenCalled_SetsSchemaFactory()
        {
            SchemaFactory schemaFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithSchemaFactory(schemaFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void WithSchemaFactory_WhenCalledWithTypeSerializerFactory_SetsSchemaFactory()
        {
            BinaryTypeSerializerFactory typeSerializerFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithSchemaFactory(typeSerializerFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }
        
        #endregion

        #region Test Methods (WithDataBufferWriterFactory)

        [Fact]
        public void WithDataBufferReaderFactory_WhenCalled_SetsDataBufferReaderFactory()
        {
            DataBufferReaderFactory dataBufferReaderFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithDataBufferReaderFactory(dataBufferReaderFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void WithDataBufferWriterFactory_WhenCalled_SetsDataBufferWriterFactory()
        {
            DataBufferWriterFactory dataBufferWriterFactory = new();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithDataBufferWriterFactory(dataBufferWriterFactory);
            Serializer<TestEntity> serializer = builder.Build();

            Assert.NotNull(serializer);
        }
        
        #endregion

        #region Test Methods (WithHeaderSerializer)

        [Fact]
        public void WithHeaderSerializer_WhenCalled_SetsDataBufferReaderFactory()
        {
            IBinaryHeaderSerializer headerSerializer = new BinaryHeaderSerializer();
            SerializerBuilder<TestEntity> builder = new();

            builder.WithHeaderSerializer(headerSerializer);
            Serializer<TestEntity> serializer = builder.Build();

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
        public void ForType_WhenCalled_ReturnNewSerializerBuilderInstance()
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