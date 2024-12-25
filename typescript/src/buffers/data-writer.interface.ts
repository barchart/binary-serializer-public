/**
 * An utilities for writing binary data to a byte array.
 *
 * @public
 * @exported
 */
export interface DataWriter {
    /**
     * Gets the total number of bytes written to the buffer.
     *
     * @public
     * @returns {number} The total number of bytes written.
     */
    get bytesWritten(): number;

    /**
     * Writes a bit to the buffer.
     *
     * @public
     * @param {boolean} value - The boolean value to write.
     * @throws {InsufficientCapacityException} if attempting to write beyond the buffer length.
     */
    writeBit(value: boolean): void;

    /**
     * Writes a byte to the buffer.
     *
     * @public
     * @param {number} value - The byte value to write.
     * @throws {InsufficientCapacityException} if attempting to write beyond the buffer length.
     */
    writeByte(value: number): void;

    /**
     * Writes bytes to the buffer.
     *
     * @public
     * @param {Uint8Array} value - The array of bytes to write.
     * @throws {InsufficientCapacityException} if attempting to write beyond the buffer length.
     */
    writeBytes(value: Uint8Array): void;

    /**
     * Converts the buffer to an array of bytes.
     *
     * @public
     * @returns {Uint8Array} The array of bytes representing the buffer's content.
     */
    toBytes(): Uint8Array;
}