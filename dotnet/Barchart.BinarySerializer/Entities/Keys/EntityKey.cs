namespace Barchart.BinarySerializer.Entities.Keys;

public class EntityKey<TEntity> : IEntityKey<TEntity>, IEquatable<EntityKey<TEntity>> where TEntity : class, new()
{
    #region Fields
    
    private readonly Object _key;
    
    #endregion
    
    #region Construtor(s)
    
    public EntityKey(Object key)
    {
        _key = key;
    }
    
    #endregion
    
    #region Methods

    public bool Equals(EntityKey<TEntity>? other)
    {
        if (other == null)
        {
            return false;
        }
        
        return _key.Equals(other._key);
    }

    public override bool Equals(object? obj)
    {
        var other = obj as EntityKey<TEntity>;

        if (other == null)
        {
            return false;
        }
        
        return Equals(other);
    }
    
    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    public override string ToString()
    {
        return $"{base.ToString()}, (key={_key})";
    }

    #endregion
}