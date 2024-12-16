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
    
    #region Test Methods (Equals with Object)
    
    [Fact]
    public void Equals_Object_SameObjectReturnsTrue()
    {
        var same = new Object();
        
        Object keyOne = new EntityKey<TestEntity>(same);
        Object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_Object_DifferentObjectsReturnsFalse()
    {
        Object keyOne = new EntityKey<TestEntity>(new Object());
        Object keyTwo = new EntityKey<TestEntity>(new Object());
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals<T> with Object)
    
    [Fact]
    public void IEquatableEquals_Object_SameObjectReturnsTrue()
    {
        var same = new Object();
        
        var keyOne = new EntityKey<TestEntity>(same);
        var keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_Object_DifferentObjectsReturnsFalse()
    {
        var keyOne = new EntityKey<TestEntity>(new Object());
        var keyTwo = new EntityKey<TestEntity>(new Object());
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals with Tuple)
    
    [Fact]
    public void Equals_Tuple_SameTupleReturnsTrue()
    {
        var same = new Tuple<String, int>("Bryan", 1);
        
        Object keyOne = new EntityKey<TestEntity>(same);
        Object keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_Tuple_DifferentTuplesSameValuesReturnsTrue()
    {
        Object keyOne = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        Object keyTwo = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void Equals_Tuple_DifferentTuplesDifferentValuesReturnsFalse()
    {
        Object keyOne = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        Object keyTwo = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 2));
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (Equals<T> with Tuple)
    
    [Fact]
    public void IEquatableEquals_Tuple_SameObjectReturnsTrue()
    {
        var same = new Tuple<String, int>("Bryan", 1);
        
        var keyOne = new EntityKey<TestEntity>(same);
        var keyTwo = new EntityKey<TestEntity>(same);
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_Tuple_DifferentTuplesSameValuesReturnsTrue()
    {
        var keyOne = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        var keyTwo = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        
        Assert.True(keyOne.Equals(keyTwo));
    }
    
    [Fact]
    public void IEquatableEquals_Tuple_DifferentTuplesDifferentValuesReturnsFalse()
    {
        var keyOne = new EntityKey<TestEntity>(new Tuple<String, int>("Bryan", 1));
        var keyTwo = new EntityKey<TestEntity>(new Tuple<String, int>("Luka", 1));
        
        Assert.False(keyOne.Equals(keyTwo));
    }
    
    #endregion
    
    #region Test Methods (ToString)
    
    [Fact]
    public void ToString_EndsWithKeyToString()
    {
        Mock<Object> mock = new();
        
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