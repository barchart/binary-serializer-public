import { Bookmark } from "./data-buffer-reader";

/**
 * An utilities for reading binary data from a byte array.
 *
 * @public
 * @exported
 */
export interface DataReader {

    /**
     * Gets the total number of bytes read from the buffer.
     *
     * @public
     * @returns {number} The total number of bytes read.
     */
    get bytesRead(): number;
    
    /**
     * Reads a single bit from the buffer.
     *
     * @public
     * @returns {boolean} The bit value read (true for 1, false for 0).
     * @throws {InsufficientCapacityException} if attempting to read beyond the buffer length.
     */
    readBit(): boolean;

    /**
     * Reads a single byte from the buffer.
     *
     * @public
     * @returns {number} The byte value read.
     * @throws {InsufficientCapacityException} if attempting to read beyond the buffer length.
     */
    readByte(): number;

    /**
     * Reads an array of bytes from the data buffer.
     *
     * @public
     * @param {number} size - The number of bytes to read.
     * @returns {Uint8Array} The array of bytes read from the buffer.
     * @throws {InsufficientCapacityException} if attempting to read beyond the buffer length.
     */
    readBytes(size: number): Uint8Array;

    /**
     * Records the current read position of the internal storage.
     *
     * @public
     * @returns {Bookmark} An `Bookmark` that causes the read position of the internal storage to be reset.
     */
    bookmark(): Bookmark;
}