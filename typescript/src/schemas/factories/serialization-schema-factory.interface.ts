import { SchemaDefinition } from "../schema-definition.interface";
import { SchemaField } from "../schema-field.type";

/**
 * Defines a factory for creating schemas for entities.
 *
 * @public
 * @exported
 */
export interface SerializationSchemaFactory {
     /**
      * Creates a schema for the specified container type.
      *
      * @public
      * @template TEntity - The type of entity for which to create the schema.
      * @param entityId - The unique identifier for the entity.
      * @param {SchemaField[]} fields - The fields defining the schema members.
      * @returns {Schema<TEntity>} An `SchemaDefinition<TEntity>` instance representing the schema for the entity.
      */
    make<TEntity extends object>(entityId: number, fields: SchemaField[]): SchemaDefinition<TEntity>;
}