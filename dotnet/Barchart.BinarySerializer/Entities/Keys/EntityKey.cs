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
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityKey{TEntity}"/> class.
    /// </summary>
    /// <param name="key">
    ///     The key value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the specified key is <see langword="null"/>.
    /// </exception>
    public EntityKey(object key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        
        _key = key;
    }
    
    #endregion
    
    #region Methods

    /// <inheritdoc />
    public bool Equals(EntityKey<TEntity>? other)
    {
        if (other == null)
        {
            return false;
        }
        
        return _key.Equals(other._key);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not EntityKey<TEntity> other)
        {
            return false;
        }
        
        return Equals(other);
    }
    
    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{base.ToString()}, (key={_key})";
    }

    #endregion
}