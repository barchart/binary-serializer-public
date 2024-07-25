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

    #region Test Methods (Serializer)

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