#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Schemas.Collections;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas.Collections;

public class SchemaItemListTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;
    
    private readonly Mock<ISchema<TestProperty>> _mockSchema;
    private readonly SchemaItemList<TestEntity, TestProperty> _schemaItemList;

    #endregion

    #region Constructor(s)

    public SchemaItemListTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        byte[] buffer = new byte[100000];
        _writer = new DataBufferWriter(buffer);
        _reader = new DataBufferReader(buffer);

        _mockSchema = new Mock<ISchema<TestProperty>>();
        _schemaItemList = new SchemaItemList<TestEntity, TestProperty>("CollectionProperty", entity => entity.ListProperty!, (entity, value) => entity.ListProperty = value.ToList()!, _mockSchema.Object);
    }

    #endregion

    #region Test Methods (Encode)
    
    [Fact]
    public void Encode_WithNonNullCollectionProperty_CallsSchemaSerializeForEachItem()
    {
        TestEntity testEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        _schemaItemList.Encode(_writer, testEntity);

        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test1" && p.PropertyValue == 123)), Times.Once);
        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test2" && p.PropertyValue == 456)), Times.Once);
    }

    [Fact]
    public void Encode_WithNullCollectionProperty_WritesNullFlag()
    {
        TestEntity testEntity = new()
        {
            ListProperty = null
        };

        _schemaItemList.Encode(_writer, testEntity);

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
            ListProperty = new List<TestProperty?>()
        };

        _mockSchema.Setup(schema => schema.Deserialize(It.IsAny<IDataBufferReader>()))
            .Callback(() => {
                TestProperty item = new()
                {
                    PropertyName = "Test",
                    PropertyValue = count++
                };
                testEntity.ListProperty.Add(item);
            })
            .Returns(() => testEntity.ListProperty.LastOrDefault()!)
            .Verifiable();

        _schemaItemList.Decode(_reader, testEntity);

        _mockSchema.Verify(schema => schema.Deserialize(It.IsAny<IDataBufferReader>()), Times.Exactly(expectedItemsCount));

        Assert.NotNull(testEntity.ListProperty);
        Assert.Equal(expectedItemsCount, testEntity.ListProperty.Count);
        
        for (int i = 0; i < expectedItemsCount; i++)
        {
            Assert.Equal(i + 1, testEntity.ListProperty.ElementAt(i)?.PropertyValue);
        }
    }

    [Fact]
    public void Decode_WithNullCollectionProperty_SetsPropertyToNull()
    {
        _writer.WriteBit(true);

        TestEntity testEntity = new()
        {
            ListProperty = new List<TestProperty?>()
        };

        _schemaItemList.Decode(_reader, testEntity);

        Assert.Empty(testEntity.ListProperty);
    } 

    #endregion
    
    #region Test Methods (CompareAndApply)

    [Fact]
    public void CompareAndApply_WithNullSourceList_DoesNothing()
    {
        TestEntity targetEntity = new()
        {
            ListProperty = new List<TestProperty?>
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
            ListProperty = null
        };

        _schemaItemList.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity.ListProperty);
        Assert.Single(targetEntity.ListProperty);
        Assert.Equal("Test1", targetEntity.ListProperty[0]?.PropertyName);
        Assert.Equal(123, targetEntity.ListProperty[0]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithNullTargetList_SetsTargetToSourceList()
    {
        TestEntity targetEntity = new()
        {
            ListProperty = null
        };

        TestEntity sourceEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        _schemaItemList.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity.ListProperty);
        Assert.Equal(2, targetEntity.ListProperty.Count);
        Assert.Equal("Test1", targetEntity.ListProperty[0]?.PropertyName);
        Assert.Equal(123, targetEntity.ListProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.ListProperty[1]?.PropertyName);
        Assert.Equal(456, targetEntity.ListProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithBothListsNonNull_UpdatesTargetWithSourceValues()
    {
        TestEntity targetEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        TestEntity sourceEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 789 },
                new() { PropertyName = "Test2", PropertyValue = 101 }
            }
        };

        _schemaItemList.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity.ListProperty);
        Assert.Equal(2, targetEntity.ListProperty.Count);
        Assert.Equal("Test1", targetEntity.ListProperty[0]?.PropertyName);
        Assert.Equal(789, targetEntity.ListProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.ListProperty[1]?.PropertyName);
        Assert.Equal(101, targetEntity.ListProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithSourceListShorter_TruncatesTargetList()
    {
        TestEntity targetEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 },
                new() { PropertyName = "Test3", PropertyValue = 789 }
            }
        };

        TestEntity sourceEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 111 },
                new() { PropertyName = "Test2", PropertyValue = 222 }
            }
        };

        _schemaItemList.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity.ListProperty);
        Assert.Equal(2, targetEntity.ListProperty.Count);
        Assert.Equal("Test1", targetEntity.ListProperty[0]?.PropertyName);
        Assert.Equal(111, targetEntity.ListProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.ListProperty[1]?.PropertyName);
        Assert.Equal(222, targetEntity.ListProperty[1]?.PropertyValue);
    }

    [Fact]
    public void CompareAndApply_WithSourceListLonger_AddsExtraItemsToTargetList()
    {
        TestEntity targetEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 }
            }
        };

        TestEntity sourceEntity = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 111 },
                new() { PropertyName = "Test2", PropertyValue = 222 },
                new() { PropertyName = "Test3", PropertyValue = 333 }
            }
        };

        _schemaItemList.CompareAndApply(ref targetEntity, sourceEntity);

        Assert.NotNull(targetEntity.ListProperty);
        Assert.Equal(3, targetEntity.ListProperty.Count);
        Assert.Equal("Test1", targetEntity.ListProperty[0]?.PropertyName);
        Assert.Equal(111, targetEntity.ListProperty[0]?.PropertyValue);
        Assert.Equal("Test2", targetEntity.ListProperty[1]?.PropertyName);
        Assert.Equal(222, targetEntity.ListProperty[1]?.PropertyValue);
        Assert.Equal("Test3", targetEntity.ListProperty[2]?.PropertyName);
        Assert.Equal(333, targetEntity.ListProperty[2]?.PropertyValue);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithBothListsNull_ReturnsTrue()
    {
        TestEntity entityA = new() 
        { 
            ListProperty = null
        };
        
        TestEntity entityB = new()
        {
            ListProperty = null
        };

        bool result = _schemaItemList.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithBothListsEmpty_ReturnsTrue()
    {
        TestEntity entityA = new() 
        { 
            ListProperty = new List<TestProperty?>() 
        };

        TestEntity entityB = new() 
        { 
            ListProperty = new List<TestProperty?>() 
        };

        bool result = _schemaItemList.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithSameItems_ReturnsTrue()
    {
        TestEntity entityA = new()
        {
            ListProperty = new List<TestProperty?>
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
            ListProperty = new List<TestProperty?>
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

        bool result = _schemaItemList.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithDifferentItems_ReturnsFalse()
    {
        TestEntity entityA = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        TestEntity entityB = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test3", PropertyValue = 789 },
                new() { PropertyName = "Test4", PropertyValue = 101 }
            }
        };

        bool result = _schemaItemList.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    [Fact]
    public void GetEquals_WithOneNullList_ReturnsFalse()
    {
        TestEntity entityA = new() { ListProperty = null };
        TestEntity entityB = new()
        {
            ListProperty = new List<TestProperty?>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 }
            }
        };

        bool result = _schemaItemList.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        public List<TestProperty?>? ListProperty { get; set; }
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