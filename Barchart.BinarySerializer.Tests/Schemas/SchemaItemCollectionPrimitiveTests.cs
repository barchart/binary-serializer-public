#region Using Statements


#endregion

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Types;

namespace Barchart.BinarySerializer.Tests.Schemas;

public class SchemaItemCollectionPrimitiveTests
{
    #region Fields

    private readonly ITestOutputHelper _testOutputHelper;

    private readonly byte[] _buffer;
    private readonly IDataBufferWriter _writer;
    private readonly IDataBufferReader _reader;

    private readonly SchemaItemCollectionPrimitive<TestEntity, int> _schemaItemListPrimitive;

    #endregion

    #region Constructor(s)

    public SchemaItemCollectionPrimitiveTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _buffer = new byte[100000];
        _writer = new DataBufferWriter(_buffer);
        _reader = new DataBufferReader(_buffer);

        IBinaryTypeSerializer<int> intSerializer = new BinarySerializerInt();

        _schemaItemListPrimitive = new SchemaItemCollectionPrimitive<TestEntity, int>("IntListProperty", entity => entity.IntListProperty!, (entity, value) => entity.IntListProperty = value?.ToList(), intSerializer);    
    }

    #endregion

    #region Test Methods (Encode)
    
    [Fact]
    public void Encode_WithNonNullListProperty_EncodesList()
    {
        TestEntity testEntity = new()
        {
            IntListProperty = new List<int> { 1, 2, 3 }
        };

        _schemaItemListPrimitive.Encode(_writer, testEntity);

        _reader.Reset();

        bool isListNull = _reader.ReadBit();
        bool isListMissing = _reader.ReadBit();

        int count = BitConverter.ToInt32(_reader.ReadBytes(4), 0);

        Assert.False(isListNull);
        Assert.False(isListMissing);

        Assert.Equal(3, count);

        Assert.False(_reader.ReadBit());
        Assert.Equal(BitConverter.GetBytes(1), _reader.ReadBytes(4));
        Assert.False(_reader.ReadBit());
        Assert.Equal(BitConverter.GetBytes(2), _reader.ReadBytes(4));
        Assert.False(_reader.ReadBit());
        Assert.Equal(BitConverter.GetBytes(3), _reader.ReadBytes(4));
    }

    [Fact]
    public void Encode_WithNullListProperty_SetsIsNullFlag()
    {
        TestEntity testEntity = new()
        {
            IntListProperty = null
        };

        _schemaItemListPrimitive.Encode(_writer, testEntity);

        _reader.Reset();

        bool isListMissing = _reader.ReadBit();
        bool isListNull = _reader.ReadBit();

        Assert.False(isListMissing);
        Assert.True(isListNull);
    }

    #endregion

    #region Test Methods (Decode)

    [Fact]
    public void Decode_WithMissingListProperty_SetsObjectPropertyToNull()
    {
        _writer.WriteBit(false);
        _writer.WriteBit(true);

        TestEntity testEntity = new()
        {
            IntListProperty = new List<int> { 1, 2, 3 }
        };

        _schemaItemListPrimitive.Decode(_reader, testEntity);

        Assert.Null(testEntity.IntListProperty);
    }

    [Fact]
    public void Decode_WithNonNullListProperty_SetsPropertyToDecodedListl()
    {
        _writer.WriteBit(false);
        _writer.WriteBit(false);
        
        _writer.WriteBytes(BitConverter.GetBytes(3));

        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(1));
        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(2));
        _writer.WriteBit(false);
        _writer.WriteBytes(BitConverter.GetBytes(3));
    
        TestEntity testEntity = new()
        {
            IntListProperty = new List<int> { 1, 2, 3 }
        };

        TestEntity decodedEntity = new() { };

        _schemaItemListPrimitive.Decode(_reader, decodedEntity);

        Assert.True(_schemaItemListPrimitive.GetEquals(testEntity, decodedEntity));
    }

    #endregion

    #region Test Methods (GetEquals)

    [Fact]
    public void GetEquals_WithIdenticalNonNullLists_ReturnsTrue()
    {
        TestEntity entity1 = new() 
        { 
            IntListProperty = new List<int> 
            { 
                1, 2, 3 
            } 
        };

        TestEntity entity2 = new() 
        { 
            IntListProperty = new List<int> 
            { 
                1, 2, 3 
            } 
        };

        bool result = _schemaItemListPrimitive.GetEquals(entity1, entity2);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithDifferentLists_ReturnsFalse()
    {
        TestEntity entity1 = new() 
        { 
            IntListProperty = new List<int> 
            { 
                1, 2, 3 
            } 
        };

        TestEntity entity2 = new() 
        { 
            IntListProperty = new List<int> 
            { 
                4, 5, 6 
            } 
        };

        bool result = _schemaItemListPrimitive.GetEquals(entity1, entity2);

        Assert.False(result);
    }

    [Fact]
    public void GetEquals_WithBothNullLists_ReturnsTrue()
    {
        TestEntity entity1 = new() 
        { 
            IntListProperty = null
        };

        TestEntity entity2 = new() 
        { 
            IntListProperty = null
        };

        bool result = _schemaItemListPrimitive.GetEquals(entity1, entity2);

        Assert.True(result);
    }

    [Fact]
    public void GetEquals_WithOneNullList_ReturnsFalse()
    {
        TestEntity entity1 = new() 
        { 
            IntListProperty = null
        };
        
        TestEntity entity2 = new() 
        { 
            IntListProperty = new List<int> { 1, 2, 3 } 
        };

        bool result = _schemaItemListPrimitive.GetEquals(entity1, entity2);

        Assert.False(result);
    }

    #endregion

    #region Nested Types

    public class TestEntity
    {
        public List<int>? IntListProperty { get; set; }
    }
    #endregion
}