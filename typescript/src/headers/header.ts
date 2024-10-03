/**
 * Represents the header of a serialized entity.
 *
 * @public
 * @exported
 * @param {number} entityId - The entity id.
 * @param {boolean} snapshot - Whether the entity is a snapshot.
 */
export class Header {
    private readonly _entityId: number;
    private readonly _snapshot: boolean;

    constructor(entityId: number, snapshot: boolean) {
        this._entityId = entityId;
        this._snapshot = snapshot;
    }

    get entityId(): number {
        return this._entityId;
    }

    get snapshot(): boolean {
        return this._snapshot;
    }
}