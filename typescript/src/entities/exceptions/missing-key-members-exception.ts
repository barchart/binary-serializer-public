/**
 * An exception thrown when an entity type does not have any properties or fields marked as keys with the `SerializeAttribute`.
 *
 * @public
 * @exported
 * @extends {Error}
 */
export class MissingKeyMembersException extends Error {
    constructor(entityType: string) {
        super(`The entity type '${entityType}' does not have any properties or fields marked as keys.`);

        Object.setPrototypeOf(this, MissingKeyMembersException.prototype);
    }
}