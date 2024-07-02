﻿#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Buffers.Factories;

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
        
        public byte[] Serialize(TEntity source, IDataBufferWriter writer)
        {
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                WriteMissingFlag(writer, false);
                    
                item.Encode(source, writer);
            }
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                WriteMissingFlag(writer, false);
                    
                item.Encode(source, writer);
            }

            return writer.ToBytes();
        }

        public void Deserialize(TEntity target, IDataBufferReader reader)
        {
            foreach (ISchemaItem<TEntity> item in _keyItems)
            {
                item.Decode(target, reader, true);
            }`
            
            foreach (ISchemaItem<TEntity> item in _valueItems)
            {
                if (ReadMissingFlag(reader))
                {
                    continue;
                }
                
                item.Decode(target, reader, true);
            }
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