#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Entities;
using Barchart.BinarySerializer.Entities.Factories;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Tests.Entities.Factories;

public class EntityManagerFactoryTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly EntityManagerFactory _factory;

    #endregion

    #region Constructor(s)

    public EntityManagerFactoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _factory = new EntityManagerFactory();
    }

    #endregion

    #region Test Methods (Make<T>)
    
    [Fact]
    public void Make_EntityWithOneKey_ReturnsEntityManagerInstance()
    {
        Mock<Serializer<TestEntityOne>> mock = new((byte)1);
        
        EntityManager<TestEntityOne> entityManager = _factory.Make(mock.Object);
        
        Assert.IsAssignableFrom<EntityManager<TestEntityOne>>(entityManager);
    }
    
    [Fact]
    public void Make_EntityWithTwoKeys_ReturnsEntityManagerInstance()
    {
        Mock<Serializer<TestEntityTwo>> mock = new((byte)1);
        
        EntityManager<TestEntityTwo> entityManager = _factory.Make(mock.Object);
        
        Assert.IsAssignableFrom<EntityManager<TestEntityTwo>>(entityManager);
    }
    
    #endregion
    
    #region Nested Types

    public class TestEntityOne
    {
        [Serialize(true)]
        public string KeyPropertyOne { get; set; } = "";
    }
    
    public class TestEntityTwo
    {
        [Serialize(true)]
        public string KeyPropertyOne { get; set; } = "";

        [Serialize(true)]
        public int KeyPropertyTwo { get; set; } = 0;
    }

    #endregion
}