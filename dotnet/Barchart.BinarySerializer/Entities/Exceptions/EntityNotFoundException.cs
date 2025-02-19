#region Using Statements

using Barchart.BinarySerializer.Entities.Keys;

#endregion

namespace Barchart.BinarySerializer.Entities.Exceptions;

/// <summary>
///     Thrown when an entity is not found in the entity manager.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity.
/// </typeparam>
public class EntityNotFoundException<TEntity> : InvalidOperationException where TEntity : class, new()
{
    #region Constructor(s)

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityNotFoundException{TEntity}"/> class.
    /// </summary>
    /// <param name="key">
    ///     The key of the entity.
    /// </param>
    public EntityNotFoundException(IEntityKey<TEntity> key) : base($"The entity manager [ {nameof(TEntity)} ] does not contain the desired entity [ ${key} ].")
    {
        
    }
    
    #endregion
}