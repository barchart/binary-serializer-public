#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas;

public class SchemaItemListPrimitive<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new()
{
    #region Fields
 
    private readonly string _name;

    private readonly Func<TEntity, List<TItem>> _getter;
    private readonly Action<TEntity, List<TItem>> _setter;

    private readonly IBinaryTypeSerializer<TItem> _elementSerializer;
    
    #endregion

    #region Constructor(s)

    public SchemaItemListPrimitive(string name, Func<TEntity, List<TItem>> getter, Action<TEntity, List<TItem>> setter, IBinaryTypeSerializer<TItem> elementSerializer)
    {
        _name = name;

        _getter = getter;
        _setter = setter;

        _elementSerializer = elementSerializer;
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => false;

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        List<TItem> items = _getter(source);
        writer.WriteBytes(BitConverter.GetBytes(items.Count));

        foreach (var item in items)
        {
            _elementSerializer.Encode(writer, item);
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        var currentItems = _getter(current);
        var previousItems = _getter(previous);

        var differentItems = currentItems.Where((item, index) => previousItems.Count <= index || !_elementSerializer.GetEquals(item, previousItems[index])).ToList();

        bool areListsEqual = currentItems.Count == previousItems.Count && !currentItems.Where((item, index) => !_elementSerializer.GetEquals(item, previousItems[index])).Any();

        WriteMissingFlag(writer, areListsEqual);

        writer.WriteBytes(BitConverter.GetBytes(differentItems.Count));

        foreach (var item in differentItems)
        {
            _elementSerializer.Encode(writer, item);
        }
        
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        bool isMissing = ReadMissingFlag(reader);

        if (isMissing)
        {
            
        }
        else 
        {
            int count = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
            
            var items = new List<TItem>();

            for (int i = 0; i < count; i++)
            {
                items.Add(_elementSerializer.Decode(reader));
            }

            _setter(target, items);
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        var listA = _getter(a);
        var listB = _getter(b);

        if (listA.Count != listB.Count) return false;

        for (int i = 0; i < listA.Count; i++)
        {
            if (!_elementSerializer.GetEquals(listA[i], listB[i])) return false;
        }

        return true;
    }

    private static bool ReadMissingFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    private static void WriteMissingFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }

    private static bool ReadNullFlag(IDataBufferReader reader)
    {
        return reader.ReadBit();
    }
    
    private static void WriteNullFlag(IDataBufferWriter writer, bool flag)
    {
        writer.WriteBit(flag);
    }
    #endregion
}