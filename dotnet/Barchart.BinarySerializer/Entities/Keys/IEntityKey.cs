namespace Barchart.BinarySerializer.Entities.Keys;

/// <summary>
///     Interface that represents a key for an entity used for (de)serialization purposes.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity.
/// </typeparam>
public interface IEntityKey<TEntity> where TEntity : class, new()
{
    
}