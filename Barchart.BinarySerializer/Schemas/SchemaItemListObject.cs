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

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, items == null);

        if(items != null)
        {
            writer.WriteBytes(BitConverter.GetBytes(items.Count));

            foreach (var item in items)
            {
                if (item != null)
                {
                    WriteNullFlag(writer, false);

                    _itemSchema.Serialize(writer, item);
                }
                else
                {
                    WriteNullFlag(writer, true);
                }
            }
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        if (GetEquals(current, previous))
        {
            WriteMissingFlag(writer, true);

            return;
        }

        List<TItem> currentItems = _getter(current);
        List<TItem> previousItems = _getter(previous);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, currentItems == null);

        if (currentItems != null)
        {
            writer.WriteBytes(BitConverter.GetBytes(currentItems.Count));

            int numberOfElements = currentItems.Count;
            
            if (previousItems != null)
            {  
                for (int i = 0; i < numberOfElements; i++)
                {
                    if (currentItems[i] == null)
                    {
                        WriteNullFlag(writer, true);
                        
                        continue;
                    }

                    WriteNullFlag(writer, false);

                    _itemSchema.Serialize(writer, currentItems[i], previousItems[i]);
                }
            }
            else 
            {
                for (int i = 0; i < numberOfElements; i++)
                {
                    if (currentItems[i] == null)
                    {
                        WriteNullFlag(writer, true);
                        continue;
                    }

                    _itemSchema.Serialize(writer, currentItems[i]);
                }
            }
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        if (ReadMissingFlag(reader))
        {
            return;
        }

        List<TItem> currentItems = _getter(target);

        if (ReadNullFlag(reader))
        {
            _setter(target, null!);

            return;
        }

        int count = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        List<TItem> items = new();

        for (int i = 0; i < count; i++)
        {
            if (ReadNullFlag(reader))
            {
                items.Add(null!);
            }
            else
            {
                TItem item = new();
                _itemSchema.Deserialize(reader, item);
                items.Add(item);
            }
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