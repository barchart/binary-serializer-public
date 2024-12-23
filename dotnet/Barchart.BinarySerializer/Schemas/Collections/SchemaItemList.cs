#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Schemas.Collections;

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
public class SchemaItemList<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new() where TItem : class, new()
{
    #region Fields

    private readonly Func<TEntity, IList<TItem>> _getter;
    private readonly Action<TEntity, IList<TItem>> _setter;

    private readonly ISchema<TItem> _itemSchema;
    
    #endregion

    #region Properties

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Key { get; }

    #endregion

    #region Constructor(s)

    public SchemaItemList(string name, Func<TEntity, IList<TItem>> getter, Action<TEntity, IList<TItem>> setter, ISchema<TItem> itemSchema)
    {
        Name = name;
        Key = false;
        
        _getter = getter;
        _setter = setter;

        _itemSchema = itemSchema;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity source)
    {
        IList<TItem> items = _getter(source);

        Serialization.WriteMissingFlag(writer, false);
        Serialization.WriteNullFlag(writer, items == null);

        if (items == null)
        {
            return;
        }
        
        writer.WriteBytes(BitConverter.GetBytes(items.Count));

        foreach (TItem item in items)
        {
            if (item != null)
            {
                Serialization.WriteNullFlag(writer, false);

                _itemSchema.Serialize(writer, item);
            }
            else
            {
                Serialization.WriteNullFlag(writer, true);
            }
        }
    }

    /// <inheritdoc />
    public void Encode(IDataBufferWriter writer, TEntity current, TEntity previous)
    {
        bool unchanged = GetEquals(current, previous);
        
        if (unchanged)
        {
            Serialization.WriteMissingFlag(writer, true);

            return;
        }

        IList<TItem> currentItems = _getter(current);
        IList<TItem> previousItems = _getter(previous);
        
        Serialization.WriteMissingFlag(writer, false);
        Serialization.WriteNullFlag(writer, currentItems == null);

        if (currentItems == null)
        {
            return;
        }
        
        writer.WriteBytes(BitConverter.GetBytes(currentItems.Count));

        int numberOfElements = currentItems.Count;
            
        for (int i = 0; i < numberOfElements; i++)
        {
            if (currentItems != null && currentItems.Count > i && currentItems[i] == null)
            {
                Serialization.WriteNullFlag(writer, true);
                    
                continue;
            }

            Serialization.WriteNullFlag(writer, false);

            if (previousItems != null && previousItems.Count > i && previousItems[i] != null)
            {
                _itemSchema.Serialize(writer, currentItems[i], previousItems[i]);
            }

            else
            {
                _itemSchema.Serialize(writer, currentItems[i]);
            }
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        if (Serialization.ReadMissingFlag(reader))
        {
            return;
        }

        if (Serialization.ReadNullFlag(reader))
        {
            _setter(target, null!);

            return;
        }

        IList<TItem> currentItems = _getter(target);

        int count = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        List<TItem> items = new();

        for (int i = 0; i < count; i++)
        {
            if (Serialization.ReadNullFlag(reader))
            {
                items.Add(null!);
            }
            else
            {
                if (currentItems != null && currentItems.Count > i && currentItems[i] != null)
                {
                    items.Add(_itemSchema.Deserialize(reader, currentItems[i]));
                }
                else
                {
                    items.Add(_itemSchema.Deserialize(reader));
                }
            }
        }

        _setter(target, items);
    }
    
    /// <inheritdoc />
    public void ApplyChanges(TEntity target, TEntity source)
    {
        if (source == null)
        {
            return;
        }
        
        IList<TItem> sourceItems = _getter(source);
        
        if (sourceItems == null)
        {
            return;
        }
        
        if (target == null)
        {
            target = new TEntity();
        }
        
        IList<TItem> targetItems = _getter(target);

        if (targetItems == null)
        {
            targetItems = new List<TItem>();
            _setter(target, targetItems);
        }

        for (int i = 0; i < sourceItems.Count; i++)
        {
            if (i < targetItems.Count)
            {
                _itemSchema.ApplyChanges(targetItems[i], sourceItems[i]);
            }
            else
            {
                targetItems.Add(sourceItems[i]);
            }
        }
    }

    /// <inheritdoc />
    public bool GetEquals(TEntity a, TEntity b)
    {
        if (a == null && b == null)
        {
            return true;
        }
        
        if (a == null || b == null)
        {
            return false;
        }
        
        IList<TItem> listA = _getter(a);
        IList<TItem> listB = _getter(b);

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

    #endregion
}