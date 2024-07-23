#region Using Statements

using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Types.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas.Factories
{
    public class SchemaFactoryTests
    {
        #region Fields

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly SchemaFactory _schemaFactory;
        private readonly Mock<IBinaryTypeSerializerFactory> _mockBinaryTypeSerializerFactory;

        #endregion

        #region Constructor(s)

        public SchemaFactoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            _mockBinaryTypeSerializerFactory = new Mock<IBinaryTypeSerializerFactory>();
            _schemaFactory = new SchemaFactory(_mockBinaryTypeSerializerFactory.Object);
        }

        #endregion
        
        #region Test Methods

        [Fact]
        public void Make_WithValidType_ReturnsSchema()
        {
            Type expectedType = typeof(TestEntity);
                
            _mockBinaryTypeSerializerFactory.Setup(f => f.Supports(typeof(int))).Returns(true);
            _mockBinaryTypeSerializerFactory.Setup(f => f.Make<int>()).Returns(Mock.Of<IBinaryTypeSerializer<int>>());
           
            ISchema<TestEntity> schema = _schemaFactory.Make<TestEntity>();

            Assert.NotNull(schema);
            Assert.IsType<Schema<TestEntity>>(schema);
        }

        #endregion

        #region Nested Types

        private class TestEntity
        {
            [Serialize]
            public int SerializedProperty { get; set; }

            public int NonSerializedProperty { get; set; }
        }

        #endregion
    }
}