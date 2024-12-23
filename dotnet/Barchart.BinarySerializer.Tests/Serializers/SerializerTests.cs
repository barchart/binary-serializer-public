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
    private readonly Serializer<Company> _multiLevelInheritenceSerializer;

    private readonly byte _entityId = 1;

    #endregion

    #region Constructor(s)

    public SerializerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper; 
        
        _serializer = new Serializer<TestEntity>(_entityId);
        _multiLevelInheritenceSerializer = new Serializer<Company>(_entityId);
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

    [Fact]
    public void Serialize_MultiLevelInheritance_ReturnsSerializedData()
    {
        Company company = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "IT",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Engineer"
                    }
                }
            }
        };
        
        byte[] serialized = _multiLevelInheritenceSerializer.Serialize(company);
        
        Assert.NotNull(serialized);
        Assert.NotEmpty(serialized);
    }

    [Fact]
    public void Serialize_MultiLevelInheritanceWithPreviousEntity_ReturnsSerializedData()
    {
        Company company = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "IT",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Engineer"
                    }
                }
            }
        };
        
        Company previousCompany = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "Engineering",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Programmer"
                    }
                }
            }
        };
        
        byte[] serialized = _multiLevelInheritenceSerializer.Serialize(company, previousCompany);
        
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
    public void Deserialize_Changes_PopulatesTargetEntity()
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

       [Fact]
    public void Deserialize_MultiLevelInheritance_PopulatesTargetEntity()
    {
        Company company = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "IT",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Engineer"
                    }
                }
            }
        };
        
        byte[] serialized = _multiLevelInheritenceSerializer.Serialize(company);
        Company deserializedCompany = _multiLevelInheritenceSerializer.Deserialize(serialized);
        
        Assert.NotNull(deserializedCompany);
        
        Assert.Equal(deserializedCompany.Name, company.Name);
        Assert.Equal(deserializedCompany.MainDepartment.DepartmentName, company.MainDepartment.DepartmentName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamName, company.MainDepartment.SubTeam.TeamName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamLeader.FullName, company.MainDepartment.SubTeam.TeamLeader.FullName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamLeader.Role, company.MainDepartment.SubTeam.TeamLeader.Role);
    }

    [Fact]
    public void Deserialize_MultiLevelInheritanceChanges_PopulatesTargetentity()
    {
        Company company = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "IT",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Engineer"
                    }
                }
            }
        };
        
        Company previousCompany = new()
        {
            Name = "Barchart",
            MainDepartment = new Company.Department
            {
                DepartmentName = "Engineering",
                SubTeam = new Company.Department.Team
                {
                    TeamName = "Development",
                    TeamLeader = new Company.Department.Team.Member
                    {
                        FullName = "Luka Sotra",
                        Role = "Programmer"
                    }
                }
            }
        };
        
        byte[] serialized = _multiLevelInheritenceSerializer.Serialize(company, previousCompany);
        Company deserializedCompany = _multiLevelInheritenceSerializer.Deserialize(serialized, previousCompany);
        
        Assert.NotNull(deserializedCompany);
        Assert.Equal(deserializedCompany.Name, company.Name);
        Assert.Equal(deserializedCompany.MainDepartment.DepartmentName, company.MainDepartment.DepartmentName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamName, company.MainDepartment.SubTeam.TeamName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamLeader.FullName, company.MainDepartment.SubTeam.TeamLeader.FullName);
        Assert.Equal(deserializedCompany.MainDepartment.SubTeam.TeamLeader.Role, company.MainDepartment.SubTeam.TeamLeader.Role);   
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

    public class Company
    {
        [Serialize(true)] 
        public string Name { get; set; } = "";

        [Serialize] 
        public Department MainDepartment { get; set; } = new();

        public class Department
        {
            [Serialize] 
            public string DepartmentName { get; set; } = "";

            [Serialize] 
            public Team SubTeam { get; set; } = new();

            public class Team
            {
                [Serialize] 
                public string TeamName { get; set; } = "";

                [Serialize] 
                public Member TeamLeader { get; set; } = new();
 
                public class Member
                {
                    [Serialize]
                    public string FullName { get; set; } = "";

                    [Serialize]
                    public string Role { get; set; } = "";
                }
            }
        }
    }
    
    #endregion
}