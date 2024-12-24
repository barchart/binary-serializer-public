#region Using Statements

using Barchart.BinarySerializer.Entities.Exceptions;
using Barchart.BinarySerializer.Entities.Keys;
using Barchart.BinarySerializer.Serializers;

#endregion

namespace Barchart.BinarySerializer.Entities;

/// <summary>
///     Manages entities by providing functionality for creating snapshots, calculating differences, and maintaining historical state.
/// </summary>
/// <typeparam name="TEntity">
///    The type of the entity.
/// </typeparam>
public class EntityManager<TEntity> where TEntity : class, new()
{
    #region Fields

    private readonly Serializer<TEntity> _serializer;
    private readonly Func<TEntity, IEntityKey<TEntity>> _keyExtractor;
    
    private readonly IDictionary<IEntityKey<TEntity>, byte[]> _snapshots;

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityManager{TEntity}"/> class.
    /// </summary>
    /// <param name="serializer">
    ///     The serializer used for serializing and deserializing entities.
    /// </param>
    /// <param name="keyExtractor">
    ///     The function that extracts the unique key for an entity.
    /// </param>
    public EntityManager(Serializer<TEntity> serializer, Func<TEntity, IEntityKey<TEntity>> keyExtractor)
    {
        _serializer = serializer;
        _keyExtractor = keyExtractor;

        _snapshots = new Dictionary<IEntityKey<TEntity>, byte[]>();
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Creates a snapshot of the given entity, serializing its current state. Optionally stores the snapshot in the internal checkpoint system.
    /// </summary>
    /// <param name="entity">
    ///     The current state of the entity.
    /// </param>
    /// <param name="checkpoint">
    ///     If true, the snapshot is stored in the internal checkpoint system.
    /// </param>
    /// <returns>
    ///     The serialized byte array representing the snapshot of the entity.
    /// </returns>
    public byte[] Snapshot(TEntity entity, bool checkpoint = true)
    {
        IEntityKey<TEntity> key = ExtractKey(entity);

        byte[] snapshot = _serializer.Serialize(entity);

        if (checkpoint)
        {
            _snapshots[key] = snapshot;
        }

        return (byte[])snapshot.Clone();
    }

    /// <summary>
    ///     Calculates the difference between the current state of an entity and its last checkpoint.
    ///     If the entity has changed, the difference is serialized and returned.
    ///     Optionally updates the checkpoint with the current state.
    /// </summary>
    /// <param name="entity">
    ///     The current state of the entity.
    /// </param>
    /// <param name="checkpoint">
    ///     If true, the current state replaces the previous checkpoint.
    /// </param>
    /// <returns>
    ///     A byte array representing the serialized difference, or an empty array if there are no changes.
    /// </returns>
    /// <exception cref="EntityNotFoundException{TEntity}">
    ///     Thrown if the entity does not have a corresponding checkpoint.
    /// </exception>
    public byte[] Difference(TEntity entity, bool checkpoint = true)
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
            return new byte[0]; // No changes
        }

        if (checkpoint)
        {
            _snapshots[key] = _serializer.Serialize(current);
        }

        return _serializer.Serialize(current, previous);
    }

    /// <summary>
    ///     Removes the snapshot of the specified entity from the internal storage.
    /// </summary>
    /// <param name="entity">
    ///     The entity object.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the snapshot was successfully removed; otherwise, <c>false</c>.
    /// </returns>
    public bool Remove(TEntity entity)
    {
        IEntityKey<TEntity> key = ExtractKey(entity);

        return _snapshots.Remove(key);
    }

    /// <summary>
    ///     Extracts the unique key for the given entity using the provided key extraction function.
    /// </summary>
    /// <param name="entity">
    ///     The entity object.
    /// </param>
    /// <returns>
    ///     The unique key for the entity.
    /// </returns>
    private IEntityKey<TEntity> ExtractKey(TEntity entity)
    {
        return _keyExtractor(entity);
    }

    #endregion
}