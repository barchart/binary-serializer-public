import { EntityKeyDefinition } from "./keys/entity-key.interface";
import { Serializer } from "../serializers/serializer";
import { EntityNotFoundException } from "./exceptions/entity-not-found-exception";

/**
 * Manages entities by providing functionality for creating snapshots, calculating differences, and maintaining historical state.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity.
 * @param {Serializer<TEntity>} serializer - The serializer used to serialize and deserialize entities.
 * @param {(entity: TEntity) => EntityKeyDefinition<TEntity>} keyExtractor - A function that extracts the key from an entity.
 *
 */
export class EntityManager<TEntity extends object> {
    private readonly _serializer: Serializer<TEntity>;
    private readonly _keyExtractor: (entity: TEntity) => EntityKeyDefinition<TEntity>;
    private readonly _snapshots: Map<EntityKeyDefinition<TEntity>, Uint8Array>;

    constructor(serializer: Serializer<TEntity>, keyExtractor: (entity: TEntity) => EntityKeyDefinition<TEntity>) {
        this._serializer = serializer;
        this._keyExtractor = keyExtractor;
        this._snapshots = new Map<EntityKeyDefinition<TEntity>, Uint8Array>();
    }

    /**
     * Creates a snapshot of the given entity, serializing its current state. Optionally stores the snapshot in the internal checkpoint system.
     *
     * @public
     * @param {TEntity} entity - The current state of the entity.
     * @param {boolean} [checkpoint] - Optional, if true, the snapshot is stored in the internal checkpoint system.
     * @returns {Uint8Array} The serialized byte array representing the snapshot of the entity.
     */
    snapshot(entity: TEntity, checkpoint: boolean = true): Uint8Array {
        const key = this.extractKey(entity);
        const snapshot = this._serializer.serialize(entity);

        if (checkpoint) {
            this._snapshots.set(key, snapshot);
        }

        return new Uint8Array(snapshot);
    }

    /**
     * Calculates the difference between the current state of an entity and its last checkpoint.
     * If the entity has changed, the difference is serialized and returned.
     * Optionally updates the checkpoint with the current state.
     *
     * @public
     * @param {TEntity} entity - The current state of the entity.
     * @param  {boolean} [checkpoint] - Optional, if true, the current state replaces the previous checkpoint.
     * @returns {Uint8Array} A byte array representing the serialized difference, or an empty array if there are no changes.
     * @throws EntityNotFoundException<TEntity> - Thrown if the entity does not have a corresponding checkpoint.
     */
    difference(entity: TEntity, checkpoint: boolean = true): Uint8Array {
        const key = this.extractKey(entity);

        if (!this.isSnapshotAvailable(key)) {
            throw new EntityNotFoundException<TEntity>(key);
        }

        const current = entity;
        const previous = this._serializer.deserialize(this.getSnapshot(key)!);

        if (this._serializer.getEquals(current, previous)) {
            return new Uint8Array(0); // No changes
        }

        if (checkpoint) {
            this.setSnapshot(key, current);
        }

        return this._serializer.serializeChanges(current, previous);
    }

    /**
     * Removes the snapshot of the specified entity from the internal storage.
     *
     * @public
     * @param {TEntity} entity - The entity object.
     * @returns {boolean} true if the snapshot was successfully removed; otherwise, false.
     */
    remove(entity: TEntity): boolean {
        const key = this.extractKey(entity);
        return this.removeSnapshot(key);
    }

    private extractKey(entity: TEntity): EntityKeyDefinition<TEntity> {
        return this._keyExtractor(entity);
    }

    private getSnapshot(key: EntityKeyDefinition<TEntity>): Uint8Array | undefined {
        for (const [snapshotKey, value] of this._snapshots.entries()) {
            if (key.equals(snapshotKey)) {
                return value;
            }
        }
        return undefined;
    }

    private setSnapshot(key: EntityKeyDefinition<TEntity>, entity: TEntity): void {
        const serializedEntity = this._serializer.serialize(entity);

        for (const snapshotKey of this._snapshots.keys()) {
            if (key.equals(snapshotKey)) {
                this._snapshots.delete(snapshotKey);
                break;
            }
        }

        this._snapshots.set(key, serializedEntity);
    }

    private removeSnapshot(key: EntityKeyDefinition<TEntity>): boolean {
        for (const snapshotKey of this._snapshots.keys()) {
            if (key.equals(snapshotKey)) {
                return this._snapshots.delete(snapshotKey);
            }
        }
        return false;
    }

    private isSnapshotAvailable(key: EntityKeyDefinition<TEntity>): boolean {
        for (const snapshotKey of this._snapshots.keys()) {
            if (key.equals(snapshotKey)) {
                return true;
            }
        }

        return false;
    }
}