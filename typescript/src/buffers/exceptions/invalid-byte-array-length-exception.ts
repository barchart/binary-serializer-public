/**
 * Thrown when the length of a byte array is invalid.
 *
 * @public
 * @exported
 * @extends {RangeError}
 * @param {number} length - The length of the byte array.
 */
export class InvalidByteArrayLengthException extends RangeError {
    constructor(length: number) {
        super(`The byte array length must be positive. The length was ${length}.`);

        Object.setPrototypeOf(this, InvalidByteArrayLengthException.prototype);
    }
}