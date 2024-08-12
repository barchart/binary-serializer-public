#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas;

/// <summary>
///     Manages the serialization and deserialization of list or array of complex types within a binary data context.
///     This class facilitates the structured encoding and decoding of item lists, leveraging a defined schema for each item
///     to ensure data integrity and support efficient data exchange. It enables both comprehensive and differential
///     serialization strategies, optimizing data storage and transmission by focusing on the differences between item states.
/// </summary>
/// <typeparam name="TEntity">
///     The type which contains the data to be serialized. In other words,
///     this is the source of data being serialized (or the assignment
///     target of data being deserialized).
/// </typeparam>
/// <typeparam name="TItem">
///     The type of the items being serialized (which is read from the source
///     object) or deserialized (which assigned to the source object).
/// </typeparam>
public class SchemaItemCollectionObject<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new() where TItem : class, new()
{
    #region Fields
 
    private readonly string _name;

    private readonly Func<TEntity, IList<TItem?>?> _getter;
    private readonly Action<TEntity, IList<TItem?>?> _setter;

    private readonly ISchema<TItem> _itemSchema;
    
    #endregion

    #region Properties

    /// <inheritdoc />
    public string Name => _name;

    /// <inheritdoc />
    public bool Key => false;

    #endregion

    #region Constructor(s)

    public SchemaItemCollectionObject(string name, Func<TEntity, IList<TItem?>?> getter, Action<TEntity, IList<TItem?>?> setter, ISchema<TItem> itemSchema)
    {
        _name = name;

        _getter = getter;
        _setter = setter;

        _itemSchema = itemSchema;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        IList<TItem?>? items = _getter(source);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, items == null);

        if (items == null)
        {
            return;
        }
        
        writer.WriteBytes(BitConverter.GetBytes(items.Count));

        foreach (TItem? item in items)
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

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        if (GetEquals(current, previous))
        {
            WriteMissingFlag(writer, true);

            return;
        }

        IList<TItem?>? currentItems = _getter(current);
        IList<TItem?>? previousItems = _getter(previous);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, currentItems == null);

        if (currentItems == null)
        {
            return;
        }
        
        writer.WriteBytes(BitConverter.GetBytes(currentItems.Count));

        int numberOfElements = currentItems.Count;
            
        for (int i = 0; i < numberOfElements; i++)
        {
            if (currentItems[i] == null)
            {
                WriteNullFlag(writer, true);
                    
                continue;
            }

            WriteNullFlag(writer, false);

            if (previousItems?[i] != null)
            {
                _itemSchema.Serialize(writer, currentItems[i]!, previousItems[i]!);
            }

            else
            {
                _itemSchema.Serialize(writer, currentItems[i]!);
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

        if (ReadNullFlag(reader))
        {
            _setter(target, null!);

            return;
        }

        IList<TItem?>? currentItems = _getter(target);

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
                if (currentItems != null && currentItems.Count > i)
                {
                    items.Add(_itemSchema.Deserialize(reader, currentItems[i]!));
                }
                else
                {
                    items.Add(_itemSchema.Deserialize(reader));
                }
            }
        }

        _setter(target, items!);
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity? a, TEntity? b)
    {
        if (a == null && b == null)
        {
            return true;
        }
        
        if (a == null || b == null)
        {
            return false;
        }
        
        IList<TItem?>? listA = _getter(a);
        IList<TItem?>? listB = _getter(b);

        if (listA == null && listB == null)
        {
            return true;
        }

        if (listA == null || listB == null)
        {
            return false;
        }

        if (listA.Count != listB.Count)
        {
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (!_itemSchema.GetEquals(listA[i], listB[i]))
            {
                return false;
            }
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