/**
 * Thrown when attempting to change an entity's key value
 * (during deserialization) or when attempting to compare
 * two entities which have different key values (during
 * serialization).
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {string} keyName - The name of the key property.
 * @param {boolean} serializing - Indicates if the exception occurred during serialization.
 */
export class KeyMismatchException extends Error {
    constructor(keyName: string, serializing: boolean) {
        const message = serializing ? `An attempt was made to serialize the difference between two entities with different key values (${keyName}).` : `An attempt was made to alter the a key property during deserialization (${keyName}).`;
        super(message);

        Object.setPrototypeOf(this, KeyMismatchException.prototype);
    }
}