import { SchemaField, Serializer, EntityKey, EntityManager } from "../../../src";
import { MissingKeyMembersException } from "../exceptions/missing-key-members-exception";

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
     *
     * @template TEntity - The type of the entity.
     * @param {Serializer<TEntity>} serializer - The serializer used to serialize and deserialize entities.
     * @param {SchemaField[]} fields - The fields of the entity schema.
     * @returns A new entity manager.
     * @throws {MissingKeyMembersException} Thrown if the entity type does not have any properties or fields marked as keys.
     */
    make<TEntity extends object>(serializer: Serializer<TEntity>, fields: SchemaField[]): EntityManager<TEntity> {
        const keyFields = fields.filter(field => 'isKey' in field && field.isKey === true);

        if (keyFields.length === 0) {
            throw new MissingKeyMembersException(this.constructor.name);
        }

        const keyExtractor = (entity: TEntity): EntityKey<TEntity> => {
            const keyValues = keyFields.map(field => (entity as any)[field.name]);
            return new EntityKey<TEntity>(keyValues);
        };

        return new EntityManager<TEntity>(serializer, keyExtractor);
    }
}