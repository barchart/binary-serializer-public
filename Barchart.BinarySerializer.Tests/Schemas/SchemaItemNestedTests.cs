#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemNestedTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;

    private readonly SchemaItemNested<TestEntity, TestProperty> _schemaItemNested;
    private readonly Mock<ISchema<TestProperty>> _mockSchema;

    #endregion

    #region Constructor(s)

    public SchemaItemNestedTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        byte[] buffer = new byte[100000]; 
        _writer = new DataBufferWriter(buffer);
        _reader = new DataBufferReader(buffer);

        _mockSchema = new Mock<ISchema<TestProperty>>();
        _schemaItemNested = new SchemaItemNested<TestEntity, TestProperty>("NestedProperty", entity => entity.NestedProperty!,
            (entity, value) => entity.NestedProperty = value, _mockSchema.Object);
    }

    #endregion

    #region Test Methods (Encode)

    [Fact]
    public void Encode_WithNonNullNestedProperty_CallsSchemaSerialize()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new TestProperty() 
            {
                PropertyName = "Test", 
                PropertyValue = 123 
            }
        };

        _schemaItemNested.Encode(_writer, testEntity);

        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test" && p.PropertyValue == 123), true), Times.Once);
    }

    [Fact]
    public void Encode_WithNullNestedProperty_WritesNullFlag()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = null
        };

        _schemaItemNested.Encode(_writer, testEntity);

        Assert.False(_reader.ReadBit());
        Assert.True(_reader.ReadBit());
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithNonNullNestedProperty_CallsSchemaDeserialize()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new TestProperty(){
                PropertyName = "Test",
                PropertyValue = 123
            }
        };

        _mockSchema.Setup(schema => schema.Deserialize(It.IsAny<IDataBufferReader>(), It.IsAny<TestProperty>(), true))
                .Callback((IDataBufferReader _, TestProperty property, bool _) =>
                {
                    property.PropertyName = "Test";
                    property.PropertyValue = 123;
                });

        _writer.WriteBit(false);
        _writer.WriteBit(false);

        _schemaItemNested.Decode(_reader, testEntity);

        _mockSchema.Verify(schema => schema.Deserialize(It.IsAny<IDataBufferReader>(), It.IsAny<TestProperty>(), true), Times.Once);
        Assert.NotNull(testEntity.NestedProperty);
        Assert.Equal("Test", testEntity.NestedProperty.PropertyName);
        Assert.Equal(123, testEntity.NestedProperty.PropertyValue);
    }

    [Fact]
    public void Decode_WithNullNestedProperty_SetsPropertyToNull()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new TestProperty()
        };

        _writer.WriteBit(false);
        _writer.WriteBit(true); 

        _schemaItemNested.Decode(_reader, testEntity);

        Assert.Null(testEntity.NestedProperty);
    }

    [Fact]
    public void Decode_WithMissingFlag_SkipsDeserialization()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new TestProperty()
        };

        _writer.WriteBit(true);

        _schemaItemNested.Decode(_reader, testEntity);

        _mockSchema.Verify(schema => schema.Deserialize(It.IsAny<IDataBufferReader>(), It.IsAny<TestProperty>(), true), Times.Never);
        Assert.NotNull(testEntity.NestedProperty);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void Equals_WithSameReference_ReturnsTrue()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new TestProperty
            {
                PropertyName = "Name1",
                PropertyValue = 123
            }
        };
        
        TestEntity sameReference = testEntity;

        Mock<ISchema<TestProperty>> mockSchema = new();
        mockSchema.Setup(x => x.GetEquals(It.IsAny<TestProperty>(), It.IsAny<TestProperty>())).Returns((TestProperty a, TestProperty b) => a.PropertyName == b.PropertyName && a.PropertyValue == b.PropertyValue);

        SchemaItemNested<TestEntity, TestProperty> schemaItemNested = new("NestedProperty", entity => entity.NestedProperty!, (entity, value) => entity.NestedProperty = value, mockSchema.Object);

        bool result = schemaItemNested.GetEquals(testEntity, sameReference);

        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        TestEntity testEntity1 = new()
        {
            NestedProperty = new TestProperty
            {
                PropertyName = "Name1",
                PropertyValue = 123
            }
        };

        TestEntity testEntity2 = new()
        {
            NestedProperty = new TestProperty
            {
                PropertyName = "Name2",
                PropertyValue = 456
            }
        };

        Mock<ISchema<TestProperty>> mockSchema = new();
        mockSchema.Setup(x => x.GetEquals(It.IsAny<TestProperty>(), It.IsAny<TestProperty>())).Returns((TestProperty a, TestProperty b) => a.PropertyName == b.PropertyName && a.PropertyValue == b.PropertyValue);

        SchemaItemNested<TestEntity, TestProperty> schemaItemNested = new("NestedProperty", entity => entity.NestedProperty!, (entity, value) => entity.NestedProperty = value, mockSchema.Object);

        bool result = schemaItemNested.GetEquals(testEntity1, testEntity2);

        Assert.False(result);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        TestEntity testEntity1 = new()
        {
            NestedProperty = new TestProperty
            {
                PropertyName = "Name1",
                PropertyValue = 123
            }
        };

        TestEntity testEntity2 = new()
        {
            NestedProperty = new TestProperty
            {
                PropertyName = "Name1",
                PropertyValue = 123
            }   
        };

        Mock<ISchema<TestProperty>> mockSchema = new();
        mockSchema.Setup(x => x.GetEquals(It.IsAny<TestProperty>(), It.IsAny<TestProperty>())).Returns((TestProperty a, TestProperty b) => a.PropertyName == b.PropertyName && a.PropertyValue == b.PropertyValue);

        SchemaItemNested<TestEntity, TestProperty> schemaItemNested = new("NestedProperty", entity => entity.NestedProperty!, (entity, value) => entity.NestedProperty = value, mockSchema.Object);

        bool result = schemaItemNested.GetEquals(testEntity1, testEntity2);

        Assert.True(result);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        public TestProperty? NestedProperty { get; set; }
    }

    public class TestProperty
    {
        [Serialize(true)]
        public string? PropertyName { get; set; }

        [Serialize]
        public int PropertyValue { get; set; }
    }

    #endregion
}