import { EntityKeyDefinition } from "./entity-key.interface";

/**
 * Represents a unique key for an entity used in (de)serialization process.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity.
 * @implements EntityKeyDefinition<TEntity>
 * @param {object} key - The unique key of the entity.
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

        if (this._key === other._key) {
            return true;
        }

        return !this.isEmptyObject(this._key) && !this.isEmptyObject(other._key) && JSON.stringify(this._key) === JSON.stringify(other._key);
    }

    toString(): string {
        return `${Object.prototype.toString.call(this)}, (key=${this._key})`;
    }

    private isEmptyObject(obj: object): boolean {
        return Object.keys(obj).length === 0 && obj.constructor === Object;
    }

}
