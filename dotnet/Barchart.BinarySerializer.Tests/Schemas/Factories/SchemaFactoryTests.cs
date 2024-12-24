#region Using Statements

using System.Runtime.CompilerServices;
using Barchart.BinarySerializer.Schemas.Factories;
using Barchart.BinarySerializer.Types.Factories;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas.Factories;

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
        
    #region Test Methods (Make)

    [Fact]
    public void Make_WithValidTypeAndDefaultEntityId_ThrowsAnError()
    {
        _mockBinaryTypeSerializerFactory.Setup(f => f.Supports(typeof(int))).Returns(true);
        _mockBinaryTypeSerializerFactory.Setup(f => f.Make<int>()).Returns(Mock.Of<IBinaryTypeSerializer<int>>());
           
        Assert.Throws<ArgumentException>(() => _schemaFactory.Make<TestEntity>());
    }
        
    [Fact]
    public void Make_WithValidTypeAndEntityId_ReturnsSchema()
    {
        const byte entityId = 1;
            
        _mockBinaryTypeSerializerFactory.Setup(f => f.Supports(typeof(int))).Returns(true);
        _mockBinaryTypeSerializerFactory.Setup(f => f.Make<int>()).Returns(Mock.Of<IBinaryTypeSerializer<int>>());
           
        ISchema<TestEntity> schema = _schemaFactory.Make<TestEntity>(entityId);

        Assert.NotNull(schema);
        Assert.IsAssignableFrom<ISchema<TestEntity>>(schema);
    }
        
    #endregion
        
    #region Nested Types

    private class TestEntity
    {
        [Serialize]
        public int SerializedProperty { get; set; }
    }
        
        
    private class TestEntityTwo
    {
        [Serialize(true)]
        public int KeyOne { get; set; }
            
        [Serialize(true)]
        public required string KeyTwo { get; set; }
            
        [Serialize()]
        public required string PropertyOne { get; set; }
    }
        
        
    #endregion
}