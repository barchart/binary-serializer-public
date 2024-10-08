#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers;

public class SerializerTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly Serializer<TestEntity> _serializer;
    
    private readonly byte _entityId = 1;

    #endregion

    #region Constructor(s)

    public SerializerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper; 
        
        _serializer = new Serializer<TestEntity>(_entityId);
    }

    #endregion

    #region Test Methods (Serialize)

    [Fact]
    public void Serialize_SingleEntity_ReturnsSerializedData()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key",
            ValueProperty = "Value"
        };

        byte[] serialized = _serializer.Serialize(entity);

        Assert.NotNull(serialized);
        Assert.NotEmpty(serialized);
    }

    [Fact]
    public void Serialize_WithPreviousEntity_ReturnsSerializedData()
    {
        TestEntity currentEntity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value1" 
        };

        TestEntity previousEntity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value0" 
        };

        byte[] serialized = _serializer.Serialize(currentEntity, previousEntity);

        Assert.NotNull(serialized);
        Assert.NotEmpty(serialized);
    }

    #endregion

    #region Test Methods (Deserialize)

    [Fact]
    public void Deserialize_SingleEntity_ReturnsDeserializedEntity()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value" 
        };

        byte[] serialized = _serializer.Serialize(entity);

        TestEntity deserializedEntity = _serializer.Deserialize(serialized);

        Assert.NotNull(deserializedEntity);
        
        Assert.Equal(entity.KeyProperty, deserializedEntity.KeyProperty);
        Assert.Equal(entity.ValueProperty, deserializedEntity.ValueProperty);
    }

    [Fact]
    public void Deserialize_Changes_ShouldPopulateTargetEntity()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value"
        };

        byte[] serialized = _serializer.Serialize(entity);
        
        TestEntity targetEntity = new()
        {
            KeyProperty = "Key",
            ValueProperty = ""
        };

        TestEntity deserializedEntity = _serializer.Deserialize(serialized, targetEntity);

        Assert.NotNull(deserializedEntity);
        
        Assert.Equal(targetEntity, deserializedEntity);
        
        Assert.Equal(entity.KeyProperty, deserializedEntity.KeyProperty);
        Assert.Equal(entity.ValueProperty, deserializedEntity.ValueProperty);
    }

    #endregion

    #region Test Methods (ReadHeader)

    [Fact]
    public void ReadHeader_WhenCalled_ReturnsCorrectHeader()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value"
        };

        byte[] serialized = _serializer.Serialize(entity);

        Header expectedHeader = new(_entityId, true);

        Header header = _serializer.ReadHeader(serialized);

        Assert.Equal(expectedHeader, header);
    }

    #endregion
    
    #region Test Methods (ReadKey)

    [Fact]
    public void ReadKey_WhenCalled_ReturnsCorrectKey()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value"
        };

        byte[] serialized = _serializer.Serialize(entity);

        string keyName = "KeyProperty";
        string expectedKey = "Key";

        string result = _serializer.ReadKey<string>(serialized, keyName);

        Assert.Equal(expectedKey, result);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_EqualEntities_ReturnsTrue()
    {
        TestEntity entityA = new() { 
            KeyProperty = "Key", 
            ValueProperty = "Value" 
        };

        TestEntity entityB = new() { 
            KeyProperty = "Key", 
            ValueProperty = "Value" 
        };

        bool result = _serializer.GetEquals(entityA, entityB);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_DifferentEntities_ReturnsFalse()
    {

        TestEntity entityA = new() { 
            KeyProperty = "Key", 
            ValueProperty = "Value" 
        };

        TestEntity entityB = new() { 
            KeyProperty = "Key", 
            ValueProperty = "DifferentValue" 
        };

        bool result = _serializer.GetEquals(entityA, entityB);

        Assert.False(result);
    }

    #endregion    
    
    #region Nested Types

    private class TestEntity
    {
        [Serialize(true)]
        public string KeyProperty { get; init; } = "";

        [Serialize]
        public string ValueProperty { get; init; } = "";
    }

    #endregion
}