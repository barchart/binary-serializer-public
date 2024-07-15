#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
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

        public TProperty ReadKey<TProperty>(IDataBufferReader reader, string name)
        {
            reader.Reset();

            TEntity target = new TEntity();
            
            for (int i = 0; i < _keyItems.Length; i++)
            {
                ISchemaItem<TEntity> candidate = _keyItems[i];
                
                candidate.Decode(reader, target, false);
                
                if (candidate.Name == name && candidate is ISchemaItem<TEntity, TProperty> match)
                {
                    TProperty key = match.Read(target);
                    
                    reader.Reset();

                    return key;
                }
            }

            reader.Reset();
            
            throw new KeyUndefinedException(typeof(TEntity), name, typeof(TProperty));
        }
        
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