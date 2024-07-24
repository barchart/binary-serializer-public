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

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithValidData_SetsData()
    {
        Mock<IDataBufferReader> readerMock = new();
        _serializerMock.Setup(s => s.Decode(readerMock.Object)).Returns("DecodedValue");

        SchemaItem<TestEntity, string> schemaItem = new("Name", false, _getter, _setter, _serializerMock.Object);

        TestEntity target = new();

        schemaItem.Decode(readerMock.Object, target);

        Assert.Equal("DecodedValue", target.Name);
    }

    [Fact]
    public void Decode_WithKeyAndExistingMismatch_ThrowsKeyMismatchException()
    {
        Mock<IDataBufferReader> readerMock = new();

        _serializerMock.Setup(s => s.Decode(readerMock.Object)).Returns("DecodedValue");
        _serializerMock.Setup(s => s.GetEquals(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);

        TestEntity target = new() { Name = "OriginalValue" };

        Assert.Throws<KeyMismatchException>(() => schemaItem.Decode(readerMock.Object, target, true));
    }

    [Fact]
    public void Decode_WithKeyAndExistingMatch_SetsData()
    {
        Mock<IDataBufferReader> readerMock = new();
        
        string decodedValue = "DecodedValue";
        _serializerMock.Setup(s => s.Decode(readerMock.Object)).Returns(decodedValue);
        _serializerMock.Setup(s => s.GetEquals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);

        TestEntity target = new() { Name = decodedValue };

        schemaItem.Decode(readerMock.Object, target, true);

        Assert.Equal(decodedValue, target.Name);
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithIdenticalValues_ReturnsTrue()
    {
        _serializerMock.Setup(s => s.GetEquals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        SchemaItem<TestEntity, string> schemaItem = new("Name", false, _getter, _setter, _serializerMock.Object);

        TestEntity a = new() { Name = "Value" };
        TestEntity b = new() { Name = "Value" };

        bool result = schemaItem.GetEquals(a, b);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithDifferentValues_ReturnsFalse()
    {
        _serializerMock.Setup(s => s.GetEquals(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        SchemaItem<TestEntity, string> schemaItem = new("Name", false, _getter, _setter, _serializerMock.Object);

        TestEntity a = new() { Name = "ValueA" };
        TestEntity b = new() { Name = "ValueB" };

        bool result = schemaItem.GetEquals(a, b);

        Assert.False(result);
    }

    #endregion

    #region Test Methods (Read)

    [Fact]
    public void Read_WithValidData_ReturnsExpectedEntity()
    {
        byte[] data = new byte[100];
        DataBufferReader reader = new(data);

        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);

        TestEntity target = new()
        {
            Name = "CurrentValue"
        };
        
        string result = schemaItem.Read(target);

        Assert.NotNull(result);
        Assert.Equal("CurrentValue", result);
    }

    [Fact]
    public void Read_WithWrongData_ReturnsWrongWEntity()
    {
        TestEntity target = new()
        {
            Name = "CurrentValue"
        };

        TestEntity targetInvalid = new()
        {
            Name = "CurrentInvalid"
        };

        byte[] data = new byte[100];
        DataBufferReader reader = new(data);

        SchemaItem<TestEntity, string> schemaItem = new("Name", true, _getter, _setter, _serializerMock.Object);

        string result = schemaItem.Read(targetInvalid);

        Assert.NotNull(result);
        Assert.Equal("CurrentInvalid", result);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        [Serialize(true)]
        public string Name { get; set; } = "";
    }

    #endregion
}