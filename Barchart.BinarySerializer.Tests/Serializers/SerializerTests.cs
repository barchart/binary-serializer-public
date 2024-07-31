#region Using Statements

using Barchart.BinarySerializer.Attributes;
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

       _serializer = new();
        
    }

    #endregion

    #region Test Methods (Serialize)

    [Fact]
    public void Serialize_ShouldReturnSerializedData()
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
    public void Serialize_WithPreviousEntity_ShouldReturnSerializedData()
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
    public void Deserialize_ShouldReturnDeserializedEntity()
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
    public void Deserialize_WithTarget_ShouldPopulateTargetEntity()
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
    public void ReadKey_ShouldReturnCorrectKey()
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

    #region Test Methods (GetEqials)

    [Fact]
    public void GetEquals_ShouldReturnTrueForEqualEntities()
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
    public void GetEquals_ShouldReturnFalseForDifferentEntities()
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

    public class TestEntity
    {
        [Serialize(true)]
        public string KeyProperty { get; set; } = "";

        [Serialize(false)]
        public string ValueProperty { get; set; } = "";
    }

    #endregion
}