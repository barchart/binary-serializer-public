#region Using Statements

namespace Barchart.BinarySerializer.Schemas.Factories;

#endregion

/// <summary>
///     Defines a factory for creating schemas for entities.
/// </summary>
public interface ISchemaFactory
{
    #region Methods
    
    /// <summary>
    ///     Creates a schema for the specified entity type with the given entity ID.
    /// </summary>
    /// <param name="entityId">
    ///     The entity ID to be assigned to the schema.
    /// </param>
    /// <typeparam name="TEntity">
    ///     The type of entity for which to create the schema.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="ISchema{TEntity}"/> representing the schema for the entity with the given entity ID.
    /// </returns>
    /// <remarks>
    ///     This method analyzes the properties of the entity type and creates a schema based on
    ///     properties marked with the [Serialize] attribute. The resulting schema can be used
    ///     for binary serialization and deserialization of the entity.
    /// </remarks>
    ISchema<TEntity> Make<TEntity>(byte entityId = 0) where TEntity: class, new();
    
    #endregion
}