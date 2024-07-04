namespace Barchart.BinarySerializer.Schemas.Factories;

/// <summary>
///     Defines a factory for creating schemas for entities.
/// </summary>
public interface ISchemaFactory
{
    /// <summary>
    ///     Creates a schema for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of entity for which to create the schema.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="ISchema{TEntity}"/> representing the schema for the entity.
    /// </returns>
    /// <remarks>
    ///     This method analyzes the properties of the entity type and creates a schema based on
    ///     properties marked with the [Serialize] attribute. The resulting schema can be used
    ///     for binary serialization and deserialization of the entity.
    /// </remarks>
    ISchema<TEntity> Make<TEntity>() where TEntity: new();
}