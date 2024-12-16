#region Constructors

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Entities;
using Barchart.BinarySerializer.Entities.Exceptions;
using Barchart.BinarySerializer.Entities.Factories;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Tests.Entities;

public class EntityManagerTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    
    #endregion
    
    #region Constructor(s)

    public EntityManagerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #endregion
    
    #region Test Methods (Snapshot)

    [Fact]
    public void Snapshot_SingleInstance_ReturnsByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000,
            Value = 0b00001111
        };

        byte[] snapshot = entityManager.Snapshot(entity);
        
        Assert.Equal(4, snapshot.Length);
    }

    [Fact]
    public void Snapshot_CompoundKey_ReturnsByteArray()
    {
        EntityManager<TestEntityTwo> entityManager = CreateTestEntityTwoManager();
        
        TestEntityTwo entity = new TestEntityTwo
        {
            Key = 0b11110000,
            KeyTwo = "KeyTwo",
            Value = 0b00001111
        };

        byte[] snapshot = entityManager.Snapshot(entity);
        
        Assert.Equal(12, snapshot.Length);
    }
    
    #endregion
    
    #region Test Methods (Difference)

    [Fact]
    public void Difference_WithoutSnapshot_Throws()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000,
            Value = 0b00001111
        };
        
        Assert.Throws<EntityNotFoundException<TestEntity>>(() => entityManager.Difference(entity));
    }
    
    [Fact]
    public void Difference_CompoundKeyEntityWithoutSnapshot_Throws()
    {
        EntityManager<TestEntityTwo> entityManager = CreateTestEntityTwoManager();
        
        TestEntityTwo entity = new TestEntityTwo
        {
            Key = 0b11110000,
            KeyTwo = "KeyTwo",
            Value = 0b00001111
        };
        
        Assert.Throws<EntityNotFoundException<TestEntityTwo>>(() => entityManager.Difference(entity));
    }
    
    [Fact]
    public void Difference_WithSnapshot_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        byte[] snapshot = entityManager.Snapshot(entity);
        byte[] difference = entityManager.Difference(entity);

        Assert.Empty(difference);
    }
    
    [Fact]
    public void Difference_CompoundKeyEntityWithSnapshot_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntityTwo> entityManager = CreateTestEntityTwoManager();
        
        TestEntityTwo entity = new TestEntityTwo
        {
            Key = 0b11110000,
            KeyTwo = "KeyTwo",
            Value = 0b00001111
        };

        byte[] snapshot = entityManager.Snapshot(entity);
        byte[] difference = entityManager.Difference(entity);

        Assert.Empty(difference);
    }
    
    [Fact]
    public void Difference_WithSnapshotThenMutate_ReturnsByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);

        entity.Value = 0b11111000;

        byte[] difference = entityManager.Difference(entity);

        Assert.Equal(4, difference.Length);
    }
    
    [Fact]
    public void Difference_CompoundKeyEntityWithSnapshotThenMutate_ReturnsByteArray()
    {
        EntityManager<TestEntityTwo> entityManager = CreateTestEntityTwoManager();
        
        TestEntityTwo entity = new TestEntityTwo
        {
            Key = 0b11110000, 
            KeyTwo = "KeyTwo",
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);

        entity.Value = 0b11111000;

        byte[] difference = entityManager.Difference(entity);

        Assert.Equal(12, difference.Length);
    }

    
    [Fact]
    public void Difference_WithSnapshotThenMutateThenDifference_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);

        entity.Value = 0b11111000;

        byte[] differenceA = entityManager.Difference(entity);
        byte[] differenceB = entityManager.Difference(entity);
        
        Assert.Empty(differenceB);
    }
    
    [Fact]
    public void Difference_WithSnapshotThenMutateThenDifferenceThenDifference_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);

        entity.Value = 0b11111000;

        byte[] differenceA = entityManager.Difference(entity);
        byte[] differenceB = entityManager.Difference(entity);
        
        Assert.Empty(differenceB);
    }
    
    [Fact]
    public void Difference_WithSnapshotThenMutateThenDifferenceThenMutateThenDifference_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);

        entity.Value = 0b11111000;

        byte[] differenceA = entityManager.Difference(entity);
        
        entity.Value = 0b11111100;
        
        byte[] differenceB = entityManager.Difference(entity);
        
        Assert.Equal(4, differenceB.Length);
    }
    
    #endregion

    #region Test Methods (Remove)

    [Fact]
    public void Remove_WithoutSnapshot_ReturnsFalse()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000,
            Value = 0b00001111
        };
        
        Assert.False(entityManager.Remove(entity));
    }
    
    [Fact]
    public void Remove_CompoundKeyEntityWithoutSnapshot_ReturnsFalse()
    {
        EntityManager<TestEntityTwo> entityManager = CreateTestEntityTwoManager();
        
        TestEntityTwo entity = new TestEntityTwo
        {
            Key = 0b11110000,
            KeyTwo = "KeyTwo",
            Value = 0b00001111
        };
        
        Assert.False(entityManager.Remove(entity));
    }
    
    [Fact]
    public void Remove_WithSnapshot_ReturnsEmptyByteArray()
    {
        EntityManager<TestEntity> entityManager = CreateTestEntityManager();
        
        TestEntity entity = new TestEntity
        {
            Key = 0b11110000, 
            Value = 0b00001111
        };

        entityManager.Snapshot(entity);
        entityManager.Remove(entity);
        
        Assert.Throws<EntityNotFoundException<TestEntity>>(() => entityManager.Difference(entity));
    }

    #endregion
    
    #region Helper Methods
    
    private static EntityManager<TestEntity> CreateTestEntityManager()
    {
        // 2024/12/15, BRI. We should be mocking these dependencies. We are incorrectly
        // testing these objects too.
        
        SerializerBuilder<TestEntity> serializerBuilder = new SerializerBuilder<TestEntity>(1);

        EntityManagerFactory entityManagerFactory = new EntityManagerFactory();
        
        return entityManagerFactory.Make(serializerBuilder.Build());
    }
    
    private static EntityManager<TestEntityTwo> CreateTestEntityTwoManager()
    {
        // 2024/12/15, BRI. We should be mocking these dependencies. We are incorrectly
        // testing these objects too.
        
        SerializerBuilder<TestEntityTwo> serializerBuilder = new SerializerBuilder<TestEntityTwo>(2);

        EntityManagerFactory entityManagerFactory = new EntityManagerFactory();
        
        return entityManagerFactory.Make(serializerBuilder.Build());
    }
    
    #endregion
    
    #region Nested Types

    public class TestEntity
    {
        [Serialize(true)]
        public byte Key { get; set; }
        
        [Serialize(false)]
        public byte Value { get; set; }
    }
    
    public class TestEntityTwo
    {
        [Serialize(true)]
        public byte Key { get; set; }

        [Serialize(true)] 
        public string KeyTwo { get; set; } = "";
        
        [Serialize(false)]
        public byte Value { get; set; }
    }


    #endregion
}