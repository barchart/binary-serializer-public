namespace Barchart.BinarySerializer.Entities.Keys;

/// <summary>
///     Represents a unique key for an entity used in (de)serialization process.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity.
/// </typeparam>
public class EntityKey<TEntity> : IEntityKey<TEntity>, IEquatable<EntityKey<TEntity>> where TEntity : class, new()
{
    #region Fields
    
    private readonly object _key;
    
    #endregion
    
    #region Construtor(s)
    
    public EntityKey(object key)
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
        if (obj is not EntityKey<TEntity> other)
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