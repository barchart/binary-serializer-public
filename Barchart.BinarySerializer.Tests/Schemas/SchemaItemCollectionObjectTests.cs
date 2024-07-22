#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemCollectionObjectTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly byte[] _buffer;
    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;
    
    private readonly Mock<ISchema<TestProperty>> _mockSchema;
    private readonly SchemaItemCollectionObject<TestEntity, TestProperty> _schemaItemCollectionObject;

    #endregion

    #region Constructor(s)

    public SchemaItemCollectionObjectTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _buffer = new byte[100000];
        _writer = new DataBufferWriter(_buffer);
        _reader = new DataBufferReader(_buffer);

        _mockSchema = new Mock<ISchema<TestProperty>>();
        _schemaItemCollectionObject = new SchemaItemCollectionObject<TestEntity, TestProperty>("CollectionProperty", entity => entity.CollectionProperty!, (entity, value) => entity.CollectionProperty = value?.ToList(), _mockSchema.Object);
    }

    #endregion

    #region Test Methods (Encode)
    
    [Fact]
    public void Encode_WithNonNullCollectionProperty_CallsSchemaSerializeForEachItem()
    {
        TestEntity testEntity = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        _schemaItemCollectionObject.Encode(_writer, testEntity);

        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test1" && p.PropertyValue == 123)), Times.Once);
        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test2" && p.PropertyValue == 456)), Times.Once);
    }

    [Fact]
    public void Encode_WithNullCollectionProperty_WritesNullFlag()
    {
        TestEntity testEntity = new()
        {
            CollectionProperty = null
        };

        _schemaItemCollectionObject.Encode(_writer, testEntity);

        _reader.Reset();
        Assert.False(_reader.ReadBit());
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithNonNullCollectionProperty_CallsSchemaDeserializeForEachItem()
    {
        var callOrder = 0;

        _mockSchema.Setup(schema => schema.Deserialize(It.IsAny<IDataBufferReader>(), It.IsAny<TestProperty>()))
            .Callback((IDataBufferReader reader, TestProperty property) =>
            {
                if (callOrder == 0)
                {
                    property.PropertyName = "Test1";
                    property.PropertyValue = 123;
                }
                else if (callOrder == 1)
                {
                    property.PropertyName = "Test2";
                    property.PropertyValue = 456;
                }

                callOrder++;
            });

        _writer.WriteBit(false);
        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(2));

        TestEntity testEntity = new();

        _schemaItemCollectionObject.Decode(_reader, testEntity);

        Assert.NotNull(testEntity.CollectionProperty);
        Assert.Equal(2, testEntity.CollectionProperty.Count);
        Assert.Equal("Test1", testEntity.CollectionProperty[0].PropertyName);
        Assert.Equal(123, testEntity.CollectionProperty[0].PropertyValue);
        Assert.Equal("Test2", testEntity.CollectionProperty[1].PropertyName);
        Assert.Equal(456, testEntity.CollectionProperty[1].PropertyValue);
    }

    [Fact]
    public void Decode_WithNullCollectionProperty_SetsPropertyToNull()
    {
        _writer.WriteBit(true);

        TestEntity testEntity = new()
        {
            CollectionProperty = new List<TestProperty>()
        };

        _schemaItemCollectionObject.Decode(_reader, testEntity);

        Assert.Empty(testEntity.CollectionProperty);
    } 

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithBothListsNull_ReturnsTrue()
    {
        TestEntity entityA = new() 
        { 
            CollectionProperty = null
        };
        
        TestEntity entityB = new()
        {
            CollectionProperty = null
        };

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithBothListsEmpty_ReturnsTrue()
    {
        TestEntity entityA = new() 
        { 
            CollectionProperty = new List<TestProperty>() 
        };

        TestEntity entityB = new() 
        { 
            CollectionProperty = new List<TestProperty>() 
        };

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithSameItems_ReturnsTrue()
    {
        TestEntity entityA = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() 
                { 
                    PropertyName = "Test1", 
                    PropertyValue = 123 
                },
                new() 
                { 
                    PropertyName = "Test2",
                    PropertyValue = 456 
                }
            }
        };

        TestEntity entityB = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() 
                { 
                    PropertyName = "Test1", 
                    PropertyValue = 123 
                },
                new() 
                { 
                    PropertyName = "Test2", 
                    PropertyValue = 456 
                }
            }
        };

        _mockSchema.Setup(schema => schema.GetEquals(It.Is<TestProperty>(p => p.PropertyName == "Test1" && p.PropertyValue == 123), 
            It.Is<TestProperty>(p => p.PropertyName == "Test1" && p.PropertyValue == 123)))
            .Returns(true);

        _mockSchema.Setup(schema => schema.GetEquals(It.Is<TestProperty>(p => p.PropertyName == "Test2" && p.PropertyValue == 456), 
            It.Is<TestProperty>(p => p.PropertyName == "Test2" && p.PropertyValue == 456)))
            .Returns(true);

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithDifferentItems_ReturnsFalse()
    {
        TestEntity entityA = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        TestEntity entityB = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() { PropertyName = "Test3", PropertyValue = 789 },
                new() { PropertyName = "Test4", PropertyValue = 101 }
            }
        };

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    [Fact]
    public void GetEquals_WithOneNullList_ReturnsFalse()
    {
        TestEntity entityA = new() { CollectionProperty = null };
        TestEntity entityB = new()
        {
            CollectionProperty = new List<TestProperty>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 }
            }
        };

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        [Serialize(true)]

        public List<TestProperty>? CollectionProperty { get; set; }
    }

    public class TestProperty
    {
        [Serialize(true)]
        public string? PropertyName { get; set; }

        [Serialize(false)]
        public int PropertyValue { get; set; }
    }

    #endregion
}