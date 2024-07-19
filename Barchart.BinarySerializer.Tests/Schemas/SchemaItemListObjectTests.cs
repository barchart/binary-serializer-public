#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Schemas.Exceptions;
using Barchart.BinarySerializer.Attributes;

using Moq;
using Xunit;


#endregion

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemListObjectTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly byte[] _buffer;
    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;
    private readonly Mock<ISchema<TestProperty>> _mockSchema;
    private readonly SchemaItemListObject<TestEntity, TestProperty> _schemaItemListObject;

    #endregion

    #region Constructor(s)

    public SchemaItemListObjectTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _buffer = new byte[100000];
        _writer = new DataBufferWriter(_buffer);
        _reader = new DataBufferReader(_buffer);

        _mockSchema = new Mock<ISchema<TestProperty>>();
        _schemaItemListObject = new SchemaItemListObject<TestEntity, TestProperty>("ListProperty", entity => entity.ListProperty!, (entity, value) => entity.ListProperty = value, _mockSchema.Object);
    }

    #endregion

    #region Test Methods (Encode)
    
    [Fact]
    public void Encode_WithNonNullListProperty_CallsSchemaSerializeForEachItem()
    {
        TestEntity testEntity = new()
        {
            ListProperty = new List<TestProperty>
            {
                new() { PropertyName = "Test1", PropertyValue = 123 },
                new() { PropertyName = "Test2", PropertyValue = 456 }
            }
        };

        _schemaItemListObject.Encode(_writer, testEntity);

        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test1" && p.PropertyValue == 123)), Times.Once);
        _mockSchema.Verify(schema => schema.Serialize(It.IsAny<IDataBufferWriter>(), It.Is<TestProperty>(p => p.PropertyName == "Test2" && p.PropertyValue == 456)), Times.Once);
    }

    [Fact]
    public void Encode_WithNullListProperty_WritesNullFlag()
    {
        TestEntity testEntity = new()
        {
            ListProperty = null
        };

        _schemaItemListObject.Encode(_writer, testEntity);

        _reader.Reset();
        Assert.False(_reader.ReadBit());
    }

    #endregion

    #region Test Methods (Decode)
    

    #endregion

    #region Test Methods (GetEquals)


    #endregion

    #region Nested Types

    public class TestEntity
    {
        public List<TestProperty>? ListProperty { get; set; }
    }

    public class TestProperty
    {
        public string? PropertyName { get; set; }
        public int PropertyValue { get; set; }
    }

    #endregion
}