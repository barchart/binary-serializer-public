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
     * @returns {number} The total number of bytes written.
     */
    get bytesWritten(): number;

    /**
     * Writes a bit to the buffer.
     *
     * @param {boolean} value - The boolean value to write.
     */
    writeBit(value: boolean): void;

    /**
     * Writes a byte to the buffer.
     *
     * @param {number} value - The byte value to write.
     */
    writeByte(value: number): void;

    /**
     * Writes bytes to the buffer.
     *
     * @param {Uint8Array} value - The array of bytes to write.
     */
    writeBytes(value: Uint8Array): void;

    /**
     * Converts the buffer to an array of bytes.
     *
     * @returns {Uint8Array} The array of bytes representing the buffer's content.
     */
    toBytes(): Uint8Array;
}