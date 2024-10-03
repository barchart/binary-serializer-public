/**
 * Thrown when attempting to access a key property (of a schema)
 * that does not exist.
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {string} keyName - The name of the key.
 */
export class KeyUndefinedException extends Error {
    constructor(keyName: string) {
        super(`The schema does not contain a key property with the specified name [${keyName}].`);

        Object.setPrototypeOf(this, KeyUndefinedException.prototype);
    }
}