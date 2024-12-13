#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;
using Barchart.BinarySerializer.Utilities;

#endregion

namespace Barchart.BinarySerializer.Schemas.Collections;

/// <summary>
///     Manages the serialization and deserialization of a list or array of primitive types associated with an entity
///     into a binary format. This class abstracts the complexities of handling binary data conversion for lists,
///     including support for both complete and differential serialization to efficiently manage data changes.
///     It utilizes a specified serializer for the elements within the list to ensure accurate type handling.
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
public class SchemaItemListPrimitive<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new()
{
    #region Fields

    private readonly Func<TEntity, IList<TItem>> _getter;
    private readonly Action<TEntity, IList<TItem>> _setter;

    private readonly IBinaryTypeSerializer<TItem> _elementSerializer;
    
    #endregion

    #region Properties

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Key { get; }

    #endregion

    #region Constructor(s)

    public SchemaItemListPrimitive(string name, Func<TEntity, IList<TItem>> getter, Action<TEntity, IList<TItem>> setter, IBinaryTypeSerializer<TItem> elementSerializer)
    {
        Name = name;
        Key = false;

        _getter = getter;
        _setter = setter;

        _elementSerializer = elementSerializer;
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

        foreach (var item in items)
        {
            Serialization.WriteMissingFlag(writer, false);

            _elementSerializer.Encode(writer, item);
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
        Serialization.NormalizeListSizes(currentItems, previousItems);
        
        Serialization.WriteMissingFlag(writer, false);
        Serialization.WriteNullFlag(writer, currentItems == null);
            
        if (currentItems != null)
        {
            WriteItems(writer, currentItems, previousItems);
        }
    }

    /// <inheritdoc />
    public void Decode(IDataBufferReader reader, TEntity target, bool existing = false)
    {
        if (Serialization.ReadMissingFlag(reader))
        {
            return;
        }

        IList<TItem> currentItems = _getter(target);
        
        if (Serialization.ReadNullFlag(reader))
        {
            if (currentItems != null)
            {
                _setter(target, null!);
            }
            
            return;
        }
        
        List<TItem> items = new();
        
        int numberOfElements = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        for (int i = 0; i < numberOfElements; i++)
        {
            if (Serialization.ReadMissingFlag(reader))
            {
                if (currentItems != null && i < currentItems.Count)
                {
                    items.Add(currentItems[i]);
                }
                else
                {
                    items.Add(default!);
                }
            }
            else
            {
                TItem decodedItem = _elementSerializer.Decode(reader);

                if (items.Count > i)
                {
                    items[i] = decodedItem;
                }
                else
                {
                    items.Add(decodedItem);
                }
            }
        }

        _setter(target, items);
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
            if (!_elementSerializer.GetEquals(listA[i], listB[i]))
            {
                return false;
            }
        }

        return true;
    }

    private void WriteItems(IDataBufferWriter writer, IList<TItem> currentItems, IList<TItem> previousItems)
    {
        int numberOfElements = currentItems.Count;
        writer.WriteBytes(BitConverter.GetBytes(numberOfElements));

        for (int i = 0; i < numberOfElements; i++)
        {
            WriteItem(writer, currentItems[i], previousItems != null ? previousItems[i] : default!);
        }
    }

    private void WriteItem(IDataBufferWriter writer, TItem currentItem, TItem previousItem)
    {
        if (currentItem != null && previousItem != null && _elementSerializer.GetEquals(currentItem, previousItem))
        {
            Serialization.WriteMissingFlag(writer, true);
        }
        else
        { 
            Serialization.WriteMissingFlag(writer, false);
            
            _elementSerializer.Encode(writer, currentItem);
        }
    }
    
    #endregion
}