#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Attributes;

#endregion

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly byte[] _buffer;
    private readonly IDataBufferWriter _writer;

    private readonly Mock<IBinaryTypeSerializer<string>> _serializerMock;
    private readonly Func<TestEntity, string> _getter;
    private readonly Action<TestEntity, string> _setter;

    #endregion

    #region Constructor(s)

    public SchemaItemTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _buffer = new byte[100000]; 
        _writer = new DataBufferWriter(_buffer);

        _serializerMock = new Mock<IBinaryTypeSerializer<string>>();
        _getter = entity => entity.Name;
        _setter = (entity, value) => entity.Name = value;
    }

    #endregion

    #region Test Methods (Encode)

    [Fact]
    public void Encode_WithValidData_WritesData()
    {
        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);
        
        TestEntity current = new()
        {
            Name = "CurrentValue"
        };
    
        schemaItem.Encode(_writer, current);
    
        _serializerMock.Verify(s => s.Encode(It.IsAny<IDataBufferWriter>(), It.Is<string>(p => p == "CurrentValue")), Times.Once);
        _serializerMock.Verify(s => s.Encode(It.IsAny<IDataBufferWriter>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Encode_WithDifferentKeyValues_ThrowsKeyMismatchException()
    {
        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);
        
        TestEntity current = new()
        {
            Name = "CurrentValue"
        };
        
        TestEntity previous = new()
        {
            Name = "PreviousValue"
        };

        Assert.Throws<KeyMismatchException>(() => schemaItem.Encode(_writer, current, previous));
    }

    [Fact]
    public void Encode_WithIdenticalKeyValues_WritesData()
    {
        Mock<IDataBufferWriter> writerMock = new();

        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);

        string value = "SameValue";

        TestEntity current = new() 
        {
            Name = value 
        };

        TestEntity previous = new() 
        { 
            Name = value
        };

        _serializerMock.Setup(s => s.GetEquals(It.Is<string>(v => v == value), It.Is<string>(v => v == value))).Returns(true);

        schemaItem.Encode(writerMock.Object, current, previous);

        _serializerMock.Verify(s => s.Encode(It.IsAny<IDataBufferWriter>(), It.Is<string>(v => v == value)), Times.Once);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        [Serialize(true)]
        public string Name { get; set; }

        public TestEntity()
        {  
            Name = "";
        }
    }

    #endregion
}