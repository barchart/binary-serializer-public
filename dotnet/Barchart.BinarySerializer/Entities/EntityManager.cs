#region Using Statements

using Barchart.BinarySerializer.Entities.Exceptions;
using Barchart.BinarySerializer.Entities.Keys;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Entities;

public class EntityManager<TEntity> where TEntity : class, new()
{
    #region Fields
    
    private readonly Serializer<TEntity> _serializer;
    private readonly Func<TEntity, IEntityKey<TEntity>> _keyExtractor;
    
    private readonly IDictionary<IEntityKey<TEntity>, byte[]> _snapshots;
    
    #endregion
    
    #region Constructor(s)

    public EntityManager(Serializer<TEntity> serializer, Func<TEntity, IEntityKey<TEntity>> keyExtractor)
    {
        _serializer = serializer;
        _keyExtractor = keyExtractor;
        
        _snapshots = new Dictionary<IEntityKey<TEntity>, byte[]>();
    }
    
    #endregion
    
    #region Methods
    
    public byte[] Snapshot(TEntity entity)
    {
        IEntityKey<TEntity> key = ExtractKey(entity);
        
        byte[] snapshot = _serializer.Serialize(entity);
        
        _snapshots[key] = snapshot;
        
        return snapshot;
    }
    
    public byte[] Difference(TEntity entity)
    {
        IEntityKey<TEntity> key = ExtractKey(entity);

        if (!_snapshots.ContainsKey(key))
        {
            throw new EntityNotFoundException<TEntity>(key);
        }
        
        TEntity current = entity;
        TEntity previous = _serializer.Deserialize(_snapshots[key]);

        if (_serializer.GetEquals(current, previous))
        {
            return new byte[0];
        }

        _snapshots[key] = _serializer.Serialize(current);

        return _serializer.Serialize(current, previous);
    }

    public bool Remove(TEntity entity)
    {
        IEntityKey<TEntity> key = ExtractKey(entity);
        
        return _snapshots.Remove(key);
    }
    
    private IEntityKey<TEntity> ExtractKey(TEntity entity)
    {
        return _keyExtractor(entity);
    }
    
    #endregion
}