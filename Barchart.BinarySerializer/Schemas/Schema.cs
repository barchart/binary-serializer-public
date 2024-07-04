#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
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

        public TEntity Deserialize(IDataBufferReader reader)
        {
            return Deserialize(reader, new TEntity(), false);
        }

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