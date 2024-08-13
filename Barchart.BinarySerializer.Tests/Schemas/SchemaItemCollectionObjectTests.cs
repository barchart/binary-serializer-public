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

    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;
    
    private readonly Mock<ISchema<TestProperty>> _mockSchema;
    private readonly SchemaItemCollectionObject<TestEntity, TestProperty> _schemaItemCollectionObject;

    #endregion

    #region Constructor(s)

    public SchemaItemCollectionObjectTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        byte[] buffer = new byte[100000];
        _writer = new DataBufferWriter(buffer);
        _reader = new DataBufferReader(buffer);

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
            CollectionProperty = new List<TestProperty?>
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

        Assert.False(_reader.ReadBit());
        Assert.True(_reader.ReadBit());
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithNonNullCollectionProperty_CallsSchemaDeserializeForEachItem()
    {
        int expectedItemsCount = 2;
        int count = 1;

        _writer.WriteBit(false);
        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(expectedItemsCount));

        TestEntity testEntity = new()
        {
            CollectionProperty = new List<TestProperty?>()
        };

        _mockSchema.Setup(schema => schema.Deserialize(It.IsAny<IDataBufferReader>()))
            .Callback(() => {
                TestProperty item = new()
                {
                    PropertyName = "Test",
                    PropertyValue = count++
                };
                testEntity.CollectionProperty.Add(item);
            })
            .Returns(() => testEntity.CollectionProperty.LastOrDefault()!)
            .Verifiable();

        _schemaItemCollectionObject.Decode(_reader, testEntity);

        _mockSchema.Verify(schema => schema.Deserialize(It.IsAny<IDataBufferReader>()), Times.Exactly(expectedItemsCount));

        Assert.NotNull(testEntity.CollectionProperty);
        Assert.Equal(expectedItemsCount, testEntity.CollectionProperty.Count);
        
        for (int i = 0; i < expectedItemsCount; i++)
        {
            Assert.Equal(i + 1, testEntity.CollectionProperty.ElementAt(i)?.PropertyValue);
        }
    }

    [Fact]
    public void Decode_WithNullCollectionProperty_SetsPropertyToNull()
    {
        _writer.WriteBit(true);

        TestEntity testEntity = new()
        {
            CollectionProperty = new List<TestProperty?>()
        };

        _schemaItemCollectionObject.Decode(_reader, testEntity);

        Assert.Empty(testEntity.CollectionProperty);
    } 

    #endregion
    
    #region Test Methods (CompareAndApply)

    [Fact]
    public void CompareAndApply_WithNullSourceList_DoesNothing()
    {
        TestEntity? targetEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new()
                {
                    PropertyName = "Test1",
                    PropertyValue = 123
                }
            }
        };

        TestEntity sourceEntity = new()
        {
            CollectionProperty = null
        };

        _schemaItemCollectionObject.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity?.CollectionProperty);
        Assert.Single(targetEntity.CollectionProperty);
        Assert.Equal("Test1", targetEntity.CollectionProperty[0]?.PropertyName);
        Assert.Equal(123, targetEntity.CollectionProperty[0]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithNullTargetList_SetsTargetToSourceList()
    {
        TestEntity? targetEntity = new()
        {
            CollectionProperty = null
        };

        TestEntity sourceEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        _schemaItemCollectionObject.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity?.CollectionProperty);
        Assert.Equal(2, targetEntity.CollectionProperty.Count);
        Assert.Equal("Test1", targetEntity.CollectionProperty[0]?.PropertyName);
        Assert.Equal(123, targetEntity.CollectionProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.CollectionProperty[1]?.PropertyName);
        Assert.Equal(456, targetEntity.CollectionProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithBothListsNonNull_UpdatesTargetWithSourceValues()
    {
        // Arrange
        TestEntity? targetEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        TestEntity sourceEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 789 },
                new() { PropertyName = "Test2", PropertyValue = 101 }
            }
        };

        _schemaItemCollectionObject.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity?.CollectionProperty);
        Assert.Equal(2, targetEntity.CollectionProperty.Count);
        Assert.Equal("Test1", targetEntity.CollectionProperty[0]?.PropertyName);
        Assert.Equal(789, targetEntity.CollectionProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.CollectionProperty[1]?.PropertyName);
        Assert.Equal(101, targetEntity.CollectionProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithSourceListShorter_TruncatesTargetList()
    {
        TestEntity? targetEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 },
                new() { PropertyName = "Test3", PropertyValue = 789 }
            }
        };

        TestEntity sourceEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 111 },
                new() { PropertyName = "Test2", PropertyValue = 222 }
            }
        };

        _schemaItemCollectionObject.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity?.CollectionProperty);
        Assert.Equal(2, targetEntity.CollectionProperty.Count);
        Assert.Equal("Test1", targetEntity.CollectionProperty[0]?.PropertyName);
        Assert.Equal(111, targetEntity.CollectionProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.CollectionProperty[1]?.PropertyName);
        Assert.Equal(222, targetEntity.CollectionProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithSourceListLonger_AddsExtraItemsToTargetList()
    {
        TestEntity? targetEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 }
            }
        };

        TestEntity sourceEntity = new()
        {
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 111 },
                new() { PropertyName = "Test2", PropertyValue = 222 },
                new() { PropertyName = "Test3", PropertyValue = 333 }
            }
        };

        _schemaItemCollectionObject.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity?.CollectionProperty);
        Assert.Equal(3, targetEntity.CollectionProperty.Count);
        Assert.Equal("Test1", targetEntity.CollectionProperty[0]?.PropertyName);
        Assert.Equal(111, targetEntity.CollectionProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.CollectionProperty[1]?.PropertyName);
        Assert.Equal(222, targetEntity.CollectionProperty[1]?.PropertyValue);
        Assert.Equal("Test3", targetEntity.CollectionProperty[2]?.PropertyName);
        Assert.Equal(333, targetEntity.CollectionProperty[2]?.PropertyValue);
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
            CollectionProperty = new List<TestProperty?>() 
        };

        TestEntity entityB = new() 
        { 
            CollectionProperty = new List<TestProperty?>() 
        };

        bool result = _schemaItemCollectionObject.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithSameItems_ReturnsTrue()
    {
        TestEntity entityA = new()
        {
            CollectionProperty = new List<TestProperty?>
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
            CollectionProperty = new List<TestProperty?>
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
            CollectionProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        TestEntity entityB = new()
        {
            CollectionProperty = new List<TestProperty?>
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
            CollectionProperty = new List<TestProperty?>
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
        public List<TestProperty?>? CollectionProperty { get; set; }
    }

    public class TestProperty
    {
        [Serialize(true)]
        public string? PropertyName { get; init; }

        [Serialize]
        public int PropertyValue { get; init; }
    }

    #endregion
}