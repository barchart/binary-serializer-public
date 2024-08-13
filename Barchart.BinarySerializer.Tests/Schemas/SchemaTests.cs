#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaTests
{
    #region Fields
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly ISchema<TestEntity> _schema;

    #endregion

    #region Constructor(s)

    public SchemaTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        BinarySerializerString serializer = new();

        SchemaItem<TestEntity, string?> keyItem = new(
            "KeyProperty",
            true,
            entity => entity.KeyProperty,
            (entity, value) => entity.KeyProperty = value,
            serializer
        );

        SchemaItem<TestEntity, string?> valueItem = new(
            "ValueProperty",
            false,
            entity => entity.ValueProperty,
            (entity, value) => entity.ValueProperty = value,
            serializer
        );

        _schema = new Schema<TestEntity>(new ISchemaItem<TestEntity>[] { keyItem, valueItem });
    }

    #endregion

    #region Test Methods (Serialize)

    [Fact]
    public void Serialize_WithValidData_WritesDataCorrectly()
    {
        TestEntity entity = new()
        {
            KeyProperty = "Key",
            ValueProperty = "Value"
        };
        
        DataBufferWriter writer = new(new byte[100]);

        byte[] bytes =_schema.Serialize(writer, entity);

        Assert.NotEmpty(bytes);
    }

    #endregion

    #region Test Methods (Deserialize)

    [Fact]
    public void Deserialize_WithValidData_ReadsDataCorrectly()
    {
        DataBufferReader reader = new(new byte[100]);

        TestEntity result = _schema.Deserialize(reader);

        Assert.NotNull(result);
    }

    #endregion

    #region Test Methods (TryReadKey)

    [Fact]
    public void ReadKey_WithValidKey_ReturnsExpectedValue()
    {
        string expectedValue = "Key";

        TestEntity entity = new()
        {
            KeyProperty = "Key",
            ValueProperty = "Value"
        };
        DataBufferWriter writer = new(new byte[100]);

        byte[] bytes =_schema.Serialize(writer, entity);

        DataBufferReader reader = new(bytes);
        
        _schema.TryReadKey(reader, "KeyProperty", out string? result);

        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void ReadKey_WithInvalidKey_ThrowsKeyUndefinedException()
    {
        string invalidValue = "InvalidKey";

        TestEntity entity = new()
        {
            KeyProperty = "Key", 
            ValueProperty = "Value"
        };
        
        DataBufferWriter writer = new(new byte[100]);

        byte[] bytes =_schema.Serialize(writer, entity);

        DataBufferReader reader = new(bytes);
        
        _schema.TryReadKey(reader, "KeyProperty", out string? _);

        Assert.False(_schema.TryReadKey<string>(reader, invalidValue, out _));
    }

    [Fact]
    public void ReadKey_WithEmptyBuffer_ThrowsKeyUndefinedException()
    {
        string invalidKeyName = "InvalidKey";

        DataBufferReader reader = new(new byte[100]);

        
        Assert.False(_schema.TryReadKey<string>(reader, invalidKeyName, out _));
    }

    #endregion

    #region Test Methods (CompareAndApply)

    [Fact]
    public void CompareAndApply_WithNonNullSource_UpdatesTarget()
    {
        TestEntity source = new()
        {
            KeyProperty = "Hello",
            ValueProperty = "Yes"
        };

        TestEntity? target = new()
        {
            KeyProperty = "Hello",
            ValueProperty = "No"
        };

        _schema.CompareAndUpdate(ref target, source);

        Assert.Equal("Hello", target?.KeyProperty);
        Assert.Equal("Yes", target?.ValueProperty);
    }

    [Fact]
    public void CompareAndApply_WithNullSource_DoesNotUpdateTarget()
    {
        TestEntity source = new()
        {
            KeyProperty = null!,
            ValueProperty = "Yes"
        };

        TestEntity? target = new()
        {
            KeyProperty = "World",
            ValueProperty = "Yes"
        };

        _schema.CompareAndUpdate(ref target, source);

        Assert.Equal("World", target?.KeyProperty);
        Assert.Equal("Yes", target?.ValueProperty);
    }

    [Fact]
    public void CompareAndApply_WithNullTarget_ThrowsNoException()
    {
        TestEntity source = new()
        {
            KeyProperty = "Hello",
            ValueProperty = "Yes"
        };

        TestEntity? target = null;

        Exception? exception = Record.Exception(() => _schema.CompareAndUpdate(ref target, source));
        Assert.Null(exception);
    }

    [Fact]
    public void CompareAndApply_WithIdenticalSourceAndTarget_NoChangesMade()
    {
        TestEntity source = new()
        {
            KeyProperty = "Hello",
            ValueProperty = "Yes",
        };

        TestEntity? target = new()
        {
            KeyProperty = "Hello",
            ValueProperty = "Yes"
        };

        _schema.CompareAndUpdate(ref target, source);

        Assert.Equal("Hello", target?.KeyProperty);
        Assert.Equal("Yes", target?.ValueProperty);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithEqualEntities_ReturnsTrue()
    {
        TestEntity entityA = new()
        {
            KeyProperty = "value", 
            ValueProperty = "value"
        };
        
        TestEntity entityB = new()
        {
            KeyProperty = "value",
            ValueProperty = "value"
        };

        bool result = _schema.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithDifferentEntities_ReturnsFalse()
    {
        TestEntity entityA = new()
        {
            KeyProperty = "valueA", 
            ValueProperty = "value"
        };
        
        TestEntity entityB = new()
        {
            KeyProperty = "valueB", 
            ValueProperty = "value"
        };

        bool result = _schema.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    #endregion

    #region Nested Types

    private class TestEntity
    {
        [Serialize(true)]
        public string? KeyProperty { get; set; } = "";

        [Serialize]
        public string? ValueProperty { get; set; } = "";
    }

    #endregion
}