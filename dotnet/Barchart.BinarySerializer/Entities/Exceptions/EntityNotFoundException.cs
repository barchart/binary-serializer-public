using Barchart.BinarySerializer.Entities.Keys;

namespace Barchart.BinarySerializer.Entities.Exceptions;

public class EntityNotFoundException<TEntity> : InvalidOperationException where TEntity : class, new()
{
    #region Properties

    /// <summary>
    ///     The key of the entity.
    /// </summary>
    public IEntityKey<TEntity> Key { get; }

    #endregion
    
    #region Constructor(s)

    public EntityNotFoundException(IEntityKey<TEntity> key) : base($"The entity manager [ {nameof(TEntity)} ] does not contain the desired entity [ ${key} ].")
    {
        Key = key;
    }
    
    #endregion
}