#region Using Statements

using Barchart.BinarySerializer.Schemas.Exceptions;

#endregion

namespace Barchart.BinarySerializer.Schemas.Factories;

/// <summary>
///     Defines a factory for creating schemas for entities.
/// </summary>
public interface ISchemaFactory
{
    #region Methods
    
    /// <summary>
    ///     Creates a schema for the specified entity type with the given entity ID.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of entity for which to create the schema.
    /// </typeparam>
    /// <param name="entityId">
    ///     The entity ID to be assigned to the schema.
    /// </param>
    /// <returns>
    ///     An <see cref="ISchema{TEntity}"/> representing the schema for the entity with the given entity ID.
    /// </returns>
    /// <remarks>
    ///     This method analyzes the properties of the entity type and creates a schema based on
    ///     properties marked with the [Serialize] attribute. The resulting schema can be used
    ///     for binary serialization and deserialization of the entity.
    /// </remarks>
    /// <exception cref="InvalidEntityIdException">
    ///     Thrown when the entity ID is invalid (i.e. [ 0 ]).
    /// </exception>
    ISchema<TEntity> Make<TEntity>(byte entityId = 0) where TEntity: class, new();
    
    #endregion
}