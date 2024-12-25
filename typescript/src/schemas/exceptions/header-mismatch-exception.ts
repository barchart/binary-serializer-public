/**
 * Thrown when there is a mismatch between the expected and actual header during deserialization.
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {number} entityId - The entity ID found in the header.
 * @param {number} expectedEntityId - The entity ID expected in the header.
 */
export class HeaderMismatchException extends Error {
    constructor(entityId: number, expectedEntityId: number) {
        super(`The header entity ID (${entityId}) does not match the expected entity ID (${expectedEntityId}).`);

        Object.setPrototypeOf(this, HeaderMismatchException.prototype);
    }
}