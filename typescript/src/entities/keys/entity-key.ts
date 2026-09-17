import Day from "@barchart/common-js/lang/Day";
import Enum from "@barchart/common-js/lang/Enum";
import * as is from "@barchart/common-js/lang/is";
import Big from "big.js";

import { EntityKeyDefinition } from "./entity-key.interface";
import { ArgumentNullException } from "../../exceptions/argument-null-exception";

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
        if (is.nil(key) || is.undef(key)) {
            throw new ArgumentNullException("key");
        }

        this._key = key;
    }

    equals(other: EntityKeyDefinition<TEntity> | null): boolean {
        if (!(other instanceof EntityKey)) {
            return false;
        }

        return EntityKey.equalsValue(this._key, other._key);
    }

    toString(): string {
        return `${this.constructor.name}, (key=${this._key})`;
    }

    private static equalsValue(left: unknown, right: unknown): boolean {
        if (Array.isArray(left) && Array.isArray(right)) {
            return left.length === right.length && left.every((value, index) => EntityKey.equalsValue(value, right[index]));
        }

        return left === right || (is.nan(left) && is.nan(right)) || (left instanceof Date && right instanceof Date && left.getTime() === right.getTime()) || (left instanceof Day && right instanceof Day && left.getIsEqual(right)) || (left instanceof Big && right instanceof Big && left.eq(right)) || (left instanceof Enum && right instanceof Enum && left.equals(right));
    }
}
