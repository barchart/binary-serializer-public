#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Headers;
using Barchart.BinarySerializer.Schemas.Headers.Exceptions;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Tests.Serializers;

public class SerializerTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    private readonly Serializer<TestEntity> _serializer;

    #endregion

    #region Constructor(s)

    public SerializerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

       _serializer = new Serializer<TestEntity>();
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

    #region Test Methods (SerializeWithHeader)
    
    [Fact]
    public void SerializeWithHeader_SingleEntity_ReturnsSerializedData()
    {
        byte entityId = 3;
        bool snapshot = true;
        
        TestEntity entity = new() 
        { 
            KeyProperty = "Key",
            ValueProperty = "Value"
        };

        byte[] serialized = _serializer.SerializeWithHeader(entity, entityId, snapshot);

        Assert.NotNull(serialized);
        Assert.NotEmpty(serialized);
    }
    
    [Fact]
    public void SerializeWithHeader_WithPreviousEntity_ReturnsSerializedData()
    {
        byte entityId = 3;
        bool snapshot = true;
        
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

        byte[] serialized = _serializer.SerializeWithHeader(currentEntity, previousEntity, entityId, snapshot);

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

    #region Test Methods (ReadHeader)

    [Fact]
    public void ReadHeader_ValidSerializedData_ReturnsExpectedHeader()
    {
        TestEntity entity = new() 
        { 
            KeyProperty = "Key", 
            ValueProperty = "Value"
        };
        
        byte entityId = 3;
        bool snapshot = true;

        byte[] serialized = _serializer.SerializeWithHeader(entity, entityId, snapshot);

        IHeader header = _serializer.ReadHeader(serialized);

        Assert.NotNull(header);
        Assert.Equal(entityId, header.EntityId);
        Assert.Equal(snapshot, header.Snapshot);
    }

    [Fact]
    public void ReadHeader_InvalidSerializedData_ThrowsInvalidHeaderException()
    {
        byte[] invalidSerialized = { 255 };

        Assert.Throws<InvalidHeaderException>(() => _serializer.ReadHeader(invalidSerialized));
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