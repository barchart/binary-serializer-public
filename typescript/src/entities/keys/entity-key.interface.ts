/**
 * Entity key definition interface.
 *
 * @typeparam TEntity - The type of the entity.
 * @public
 * @exported
 */
export interface EntityKeyDefinition<TEntity extends object> {
    equals(other: EntityKeyDefinition<TEntity> | null): boolean;
}