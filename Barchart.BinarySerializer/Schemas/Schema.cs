#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents a schema for serializing and deserializing entities of type <typeparamref name="TEntity"/>.
    ///     This class includes functionality for converting an entity to a binary format via serialization and for converting from a binary format back to an entity through deserialization.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of the entity this schema is for. The entity must have a parameterless constructor.
    /// </typeparam>
    public class Schema<TEntity> : ISchema<TEntity> where TEntity: class, new()
    {
        #region Fields

        private readonly ISchemaItem<TEntity>[] _keyItems;
        private readonly ISchemaItem<TEntity>[] _valueItems;
        
        #endregion
        
        #region Constructor(s)
        
        public Schema(ISchemaItem<TEntity>[] items)
        {
            _keyItems = items.Where(i => i.Key).ToArray();
            _valueItems = items.Where(i => !i.Key).ToArray();
        }
        
        #endregion
        
        #region Methods
        
        /// <inheritdoc />
        public byte[] Serialize(IDataBufferWriter writer, TEntity source)
        {
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                item.Encode(writer, source);
            }
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                item.Encode(writer, source);
            }

            return writer.ToBytes();
        }
        
        /// <inheritdoc />
        public byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous)
        {
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                item.Encode(writer, current, previous);
            }

            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                item.Encode(writer, current, previous);
            }
            
            return writer.ToBytes();
        }

        /// <inheritdoc />
        public TEntity Deserialize(IDataBufferReader reader)
        {
            return Deserialize(reader, new TEntity(), false);
        }

        /// <inheritdoc />
        public TEntity Deserialize(IDataBufferReader reader, TEntity target)
        {
            return Deserialize(reader, target, true);
        }

        private TEntity Deserialize(IDataBufferReader reader, TEntity target, bool existing)
        {
            reader.Reset();
            
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                item.Decode(reader, target, existing);
            }
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                item.Decode(reader, target, existing);
            }
            
            reader.Reset();

            return target;
        }
        
        public bool GetEquals(TEntity a, TEntity b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a != null && b != null)
            {
                return _keyItems.All(si => si.GetEquals(a, b)) && _valueItems.All(si => si.GetEquals(a, b));
            }

            return false;
        }
        
        #endregion
    }
}