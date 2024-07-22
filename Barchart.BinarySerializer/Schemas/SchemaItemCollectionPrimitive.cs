#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas;

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
public class SchemaItemCollectionPrimitive<TEntity, TItem> : ISchemaItem<TEntity> where TEntity : class, new()
{
    #region Fields
 
    private readonly string _name;

    private readonly Func<TEntity, IList<TItem>> _getter;
    private readonly Action<TEntity, IList<TItem>> _setter;

    private readonly IBinaryTypeSerializer<TItem> _elementSerializer;
    
    #endregion

    #region Constructor(s)

    public SchemaItemCollectionPrimitive(string name, Func<TEntity, IList<TItem>> getter, Action<TEntity, IList<TItem>> setter, IBinaryTypeSerializer<TItem> elementSerializer)
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
        IList<TItem> items = _getter(source);
        
        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, items == null);

        if (items != null)
        {
            writer.WriteBytes(BitConverter.GetBytes(items.Count));

            foreach (var item in items)
            {
                WriteMissingFlag(writer, false);

                _elementSerializer.Encode(writer, item);
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

        IList<TItem> currentItems = _getter(current);
        IList<TItem> previousItems = _getter(previous);

        WriteMissingFlag(writer, false);
        WriteNullFlag(writer, currentItems == null);
            
        if (currentItems != null)
        {
            int numberOfElements = currentItems.Count;

            writer.WriteBytes(BitConverter.GetBytes(numberOfElements));
            
            for (int i = 0; i < numberOfElements; i++)
            {
                if (_elementSerializer.GetEquals(currentItems[i], previousItems[i]))
                {
                    WriteMissingFlag(writer, true);
                }
                else
                {
                    WriteMissingFlag(writer, false);
                    
                    _elementSerializer.Encode(writer, currentItems[i]);
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

        if (ReadNullFlag(reader))
        {
            if (!existing)
            {
                _setter(target, null!);
            }
            
            return;
        }
        
        int numberOfElements = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)));
        
        IList<TItem> items = existing && _getter(target) != null ? _getter(target) : new List<TItem>();
    
        for (int i = 0; i < numberOfElements; i++)
        {
            if (ReadMissingFlag(reader))
            {
                continue;
            }
            else
            {
                TItem decodedItem = _elementSerializer.Decode(reader);

                if (existing && items.Count > i)
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
        IList<TItem> listA = _getter(a);
        IList<TItem> listB = _getter(b);

        if (listA == null && listB == null) return true;
        if (listA == null || listB == null) return false;

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