/**
 * Entity key definition interface.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity.
 */
export interface EntityKeyDefinition<TEntity extends object> {

    /**
     * Checks if this key is equal to another key.
     *
     * @public
     * @param {EntityKeyDefinition<TEntity> | null} other - The other key to compare.
     * @returns {boolean} True if the keys are equal, false otherwise.
     */
    equals(other: EntityKeyDefinition<TEntity> | null): boolean;
}