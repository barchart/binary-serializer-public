import { EntityKeyDefinition } from "../keys/entity-key.interface";

/**
 * Thrown when an entity is not found in the entity manager.
 *
 * @typeparam TEntity - The type of the entity.
 */
export class EntityNotFoundException<TEntity extends object> extends Error {
    constructor(key: EntityKeyDefinition<TEntity>) {
        super(`The entity manager does not contain the desired entity [ ${key} ].`);

        Object.setPrototypeOf(this, EntityNotFoundException.prototype);
    }
}