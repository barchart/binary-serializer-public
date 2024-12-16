#region Using Statements

using Barchart.BinarySerializer.Entities.Keys;

#endregion

namespace Barchart.BinarySerializer.Tests.Entities.Keys;

public class EntityKeyTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    #endregion

    #region Constructor(s)

    public EntityKeyTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #endregion
    
    #region Test Methods (Equals with object)
    
    [Fact]
    public void Equals_object_SameObjectReturnsTrue()
    {
        object same = new();
        
        object keyOne = new EntityKey<TestEntity>(same);
        object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_object_DifferentObjectsReturnsFalse()
    {
        object keyOne = new EntityKey<TestEntity>(new object());
        object keyTwo = new EntityKey<TestEntity>(new object());
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals<T> with object)
    
    [Fact]
    public void IEquatableEquals_object_SameObjectReturnsTrue()
    {
        object same = new();
        
        object keyOne = new EntityKey<TestEntity>(same);
        object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_object_DifferentObjectsReturnsFalse()
    {
        object keyOne = new EntityKey<TestEntity>(new object());
        object keyTwo = new EntityKey<TestEntity>(new object());
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals with Tuple)
    
    [Fact]
    public void Equals_Tuple_SameTupleReturnsTrue()
    {
        object same = new Tuple<string, int>("Bryan", 1);
        
        object keyOne = new EntityKey<TestEntity>(same);
        object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_Tuple_DifferentTuplesSameValuesReturnsTrue()
    {
        object keyOne = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        object keyTwo = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_Tuple_DifferentTuplesDifferentValuesReturnsFalse()
    {
        object keyOne = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        object keyTwo = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 2));
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals<T> with Tuple)
    
    [Fact]
    public void IEquatableEquals_Tuple_SameObjectReturnsTrue()
    {
        object same = new Tuple<string, int>("Bryan", 1);
        
        object keyOne = new EntityKey<TestEntity>(same);
        object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_Tuple_DifferentTuplesSameValuesReturnsTrue()
    {
        object keyOne = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        object keyTwo = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_Tuple_DifferentTuplesDifferentValuesReturnsFalse()
    {
        object keyOne = new EntityKey<TestEntity>(new Tuple<string, int>("Bryan", 1));
        object keyTwo = new EntityKey<TestEntity>(new Tuple<string, int>("Luka", 1));
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (ToString)
    
    [Fact]
    public void ToString_EndsWithKeyToString()
    {
        Mock<object> mock = new();
        
        mock.Setup(o => o.ToString()).Returns("The End");
        
        EntityKey<TestEntity> key = new(mock.Object);
        
        Assert.EndsWith("(key=The End)", key.ToString());
    }
    
    #endregion
    
    #region Nested Types

    public class TestEntity
    {
        
    }

    #endregion
}