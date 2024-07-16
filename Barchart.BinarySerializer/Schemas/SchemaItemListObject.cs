#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

public class SchemaItemListObject<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new() where TItem : class, new()
{
    #region Fields
 
    private readonly string _name;

    private readonly Func<TEntity, List<TItem>> _getter;
    private readonly Action<TEntity, List<TItem>> _setter;

    private readonly ISchema<TItem> _itemSchema;
    
    #endregion

    #region Constructor(s)

    public SchemaItemListObject(string name, Func<TEntity, List<TItem>> getter, Action<TEntity, List<TItem>> setter, ISchema<TItem> itemSchema)
    {
        _name = name;

        _getter = getter;
        _setter = setter;

        _itemSchema = itemSchema;
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

        if (items == null)
        {
            WriteMissingFlag(writer, true);

            return;
        }

        WriteMissingFlag(writer, false);

        writer.WriteBytes(BitConverter.GetBytes(items.Count));

        foreach (var item in items)
        {
            _itemSchema.Serialize(writer, item);
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        List<TItem> currentItems = _getter(current);
        List<TItem> previousItems = _getter(previous);

        if (currentItems == null && previousItems == null)
        {
            WriteMissingFlag(writer, true);

            return;
        }

        if (currentItems == null)
        {
            WriteMissingFlag(writer, false);

            writer.WriteBytes(BitConverter.GetBytes(0));
            
            return;
        }

        List<TItem> differentItems = currentItems.Where((item, index) => previousItems.Count <= index || !_itemSchema.GetEquals(item, previousItems[index])).ToList();

        bool areListsEqual = currentItems.Count == previousItems.Count && !currentItems.Where((item, index) => !_itemSchema.GetEquals(item, previousItems[index])).Any();

        if (areListsEqual)
        {
            WriteMissingFlag(writer, true);
            
            return;
        }

        WriteMissingFlag(writer, false);

        writer.WriteBytes(BitConverter.GetBytes(differentItems.Count));

        foreach (var item in differentItems)
        {
            _itemSchema.Serialize(writer, item);
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        bool isMissing = ReadMissingFlag(reader);

        if (isMissing)
        {
            return;
        }

        int count = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        List<TItem> items = new();

        for (int i = 0; i < count; i++)
        {
            TItem item = new();
            _itemSchema.Deserialize(reader, item);
            
            items.Add(item);
        }

        _setter(target, items);
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        List<TItem> listA = _getter(a);
        List<TItem> listB = _getter(b);

        if (listA == null && listB == null) return true;
        if (listA == null || listB == null) return false;
        
        if (listA.Count != listB.Count) return false;

        for (int i = 0; i < listA.Count; i++)
        {
            if (!_itemSchema.GetEquals(listA[i], listB[i])) return false;
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

    #endregion
}