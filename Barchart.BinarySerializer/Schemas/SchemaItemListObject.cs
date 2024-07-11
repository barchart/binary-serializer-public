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
        writer.WriteBytes(BitConverter.GetBytes(items.Count));

        foreach (var item in items)
        {
            _itemSchema.Serialize(writer, item);
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        var currentItems = _getter(current);
        var previousItems = _getter(previous);

        var differentItems = currentItems.Where((item, index) => 
            previousItems.Count <= index || !_itemSchema.GetEquals(item, previousItems[index])).ToList();

        writer.WriteBytes(BitConverter.GetBytes(differentItems.Count));

        foreach (var item in differentItems)
        {
            _itemSchema.Serialize(writer, item);
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        int count = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        var items = new List<TItem>();

        for (int i = 0; i < count; i++)
        {
            var item = new TItem();
            _itemSchema.Deserialize(reader, item);
            items.Add(item);
        }

        _setter(target, items);
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        var listA = _getter(a);
        var listB = _getter(b);

        if (listA.Count != listB.Count) return false;

        for (int i = 0; i < listA.Count; i++)
        {
            if (!_itemSchema.GetEquals(listA[i], listB[i])) return false;
        }

        return true;
    }

    #endregion
}