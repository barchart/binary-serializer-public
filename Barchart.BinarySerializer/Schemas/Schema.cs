#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;

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
    public class Schema<TEntity> : ISchema<TEntity> where TEntity: new()
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
                item.Encode(source, writer);
            }
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                WriteMissingFlag(writer, false);
                    
                item.Encode(source, writer);
            }

            return writer.ToBytes();
        }
        
        /// <inheritdoc />
        public byte[] Serialize(IDataBufferWriter writer, TEntity current, TEntity previous)
        {
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                bool keysAreEqual = item.GetEquals(current, previous);

                if (keysAreEqual)
                {
                    item.Encode(current, writer);
                }
                else
                {
                    throw new KeyMismatchException(item.Name, true);
                }
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
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                item.Decode(target, reader, existing);
            }
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                if (ReadMissingFlag(reader)) 
                {
                    continue;
                }
                
                item.Decode(target, reader, existing);
            }

            return target;
        }
        
        private static bool ReadMissingFlag(IDataBufferReader reader)
        {
            return reader.ReadBit();
        }
    
        private static void WriteMissingFlag(IDataBufferWriter writer, bool missing)
        {
            writer.WriteBit(missing);
        }
        
        #endregion
    }
}