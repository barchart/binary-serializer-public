#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Buffers;

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

    [Fact]
    public void Decode_WithNonNullListProperty_CallsSchemaDeserializeForEachItem()
    {
        var callOrder = 0;

        _mockSchema.Setup(schema => schema.Deserialize(It.IsAny<IDataBufferReader>(), It.IsAny<TestProperty>()))
            .Callback((IDataBufferReader reader, TestProperty property) =>
            {
                if (callOrder == 0)
                {
                    property.PropertyName = "Test1";
                    property.PropertyValue = 123;
                }
                else if (callOrder == 1)
                {
                    property.PropertyName = "Test2";
                    property.PropertyValue = 456;
                }

                callOrder++;
            });

        _writer.WriteBit(false);
        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(2));

        TestEntity testEntity = new();

        _schemaItemListObject.Decode(_reader, testEntity);

        Assert.NotNull(testEntity.ListProperty);
        Assert.Equal(2, testEntity.ListProperty.Count);
        Assert.Equal("Test1", testEntity.ListProperty[0].PropertyName);
        Assert.Equal(123, testEntity.ListProperty[0].PropertyValue);
        Assert.Equal("Test2", testEntity.ListProperty[1].PropertyName);
        Assert.Equal(456, testEntity.ListProperty[1].PropertyValue);
    }

    [Fact]
    public void Decode_WithNullListProperty_SetsPropertyToNull()
    {
        _writer.WriteBit(true);

        TestEntity testEntity = new()
        {
            ListProperty = new List<TestProperty>()
        };

        _schemaItemListObject.Decode(_reader, testEntity);

        Assert.Empty(testEntity.ListProperty);
    } 

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