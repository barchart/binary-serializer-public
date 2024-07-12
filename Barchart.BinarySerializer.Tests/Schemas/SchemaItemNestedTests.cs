using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Attributes;
using Moq;
using Xunit;

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemNestedTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly byte[] _buffer;
    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;

    private readonly SchemaItemNested<TestEntity, TestProperty> _schemaItemNested;
    private readonly Mock<ISchema<TestProperty>> _mockSchema;

    #endregion

    #region Constructor(s)

    public SchemaItemNestedTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _buffer = new byte[100000]; 
        _writer = new DataBufferWriter(_buffer);
        _reader = new DataBufferReader(_buffer);

        _mockSchema = new Mock<ISchema<TestProperty>>();
        _schemaItemNested = new SchemaItemNested<TestEntity, TestProperty>("NestedProperty", entity => entity.NestedProperty!,
            (entity, value) => entity.NestedProperty = value, _mockSchema.Object);
    }

    #endregion

    #region Test Methods (Encode)

     [Fact]
    public void Encode_WithNonNullNestedProperty_CallsSchemaSerialize()
    {
        TestEntity testEntity = new()
        {
            NestedProperty = new() 
            {
                PropertyName = "Test", 
                PropertyValue = 123 
            }
        };

        _schemaItemNested.Encode(_writer, testEntity);

        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test" && p.PropertyValue == 123)), Times.Once);
    }

    [Fact]
    public void Encode_WithNullNestedProperty_WritesNullFlag()
    {
        _reader.Reset();

        TestEntity testEntity = new()
        {
            NestedProperty = null
        };

        _schemaItemNested.Encode(_writer, testEntity);

        Assert.False(_reader.ReadBit());
        Assert.True(_reader.ReadBit());
    }

    [Fact]
    public void Encode_WithDifferentKeyValues_ThrowsKeyMismatchException()
    {
    }

    [Fact]
    public void Encode_WithIdenticalKeyValues_WritesData()
    {
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithValidData_SetsData()
    {
    }

    [Fact]
    public void Decode_WithKeyAndExistingMismatch_ThrowsKeyMismatchException()
    {
    }

    [Fact]
    public void Decode_WithKeyAndExistingMatch_SetsData()
    {
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithIdenticalValues_ReturnsTrue()
    {
    }

    [Fact]
    public void GetEquals_WithDifferentValues_ReturnsFalse()
    {
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        public TestProperty? NestedProperty { get; set; }

        public TestEntity()
        {  
        }
    }

    public class TestProperty
    {
        [Serialize(true)]
        public string? PropertyName { get; set; }

        [Serialize(false)]
        public int PropertyValue { get; set; }

        public TestProperty()
        {
        }
    }

    #endregion
}