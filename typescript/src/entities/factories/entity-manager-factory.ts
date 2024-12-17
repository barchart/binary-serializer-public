import { SchemaField } from "../../schemas/schema-field.type";
import { Serializer } from "../../serializers/serializer";
import { EntityManager } from "../entity-manager";
import { EntityKey } from "../keys/entity-key";

/**
 * A factory for creating entity managers.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity.
 */
export class EntityManagerFactory {

    /**
     * Creates a new entity manager for the given serializer and fields.
     * @template TEntity - The type of the entity.
     * @param {Serializer<TEntity>} serializer - The serializer used to serialize and deserialize entities.
     * @param {SchemaField[]} fields - The fields of the entity schema.
     * @returns A new entity manager.
     */
    make<TEntity extends object>(serializer: Serializer<TEntity>, fields: SchemaField[]): EntityManager<TEntity> {
        const keyExtractor = (entity: TEntity): EntityKey<TEntity> => {
            const keyValues = fields
                .filter(field => 'isKey' in field && field.isKey === true)
                .map(field => (entity as any)[field.name]);

            return new EntityKey<TEntity>(keyValues);
        };

        return new EntityManager<TEntity>(serializer, keyExtractor);
    }
}