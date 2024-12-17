import { EntityKeyDefinition } from "./entity-key.interface";

/**
 * Represents a unique key for an entity used in (de)serialization process.
 *
 * @typeparam TEntity - The type of the entity.
 */
export class EntityKey<TEntity extends object> implements EntityKeyDefinition<TEntity> {
    private readonly _key: object;

    constructor(key: object) {
        this._key = key;
    }

    equals(other: EntityKey<TEntity> | null): boolean {
        if (other === null) {
            return false;
        }

        return this._key === other._key;
    }

    toString(): string {
        return `${Object.prototype.toString.call(this)}, (key=${this._key})`;
    }
}
