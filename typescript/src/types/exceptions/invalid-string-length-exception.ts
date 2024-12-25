/**
 * Thrown when the length of a string is invalid.
 *
 * @public
 * @exported
 * @param {number} length - The length of the string.
 * @param {number} maximumStringLengthInBytes - The maximum length of a string.
 */
export class InvalidStringLengthException extends Error {
    constructor(length: number, maximumStringLengthInBytes: number) {
        super(`Unable to serialize string. Serialized string would require ${length} bytes; however, the maximum size of a serialized string is ${maximumStringLengthInBytes}`);

        Object.setPrototypeOf(this, InvalidStringLengthException.prototype);
    }
}
