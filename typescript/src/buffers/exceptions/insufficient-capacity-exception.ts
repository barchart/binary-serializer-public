/**
 * Thrown when attempting to change an entity's key value (during deserialization) or when attempting to compare
 * two entities which have different key values (during serialization).
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {boolean} writing - Indicates whether the operation is a write or read.
 */
export class InsufficientCapacityException extends Error {
    constructor(writing: boolean) {
        super(writing ? "Unable to write to [DataBufferWriter], remaining capacity would be exceeded." : "Unable to read from [DataBufferReader], remaining capacity would be exceeded.");
        
        Object.setPrototypeOf(this, InsufficientCapacityException.prototype);
    }
}