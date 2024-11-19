import { Bookmark } from "./data-buffer-reader";

/**
 * An utilities for reading binary data from a byte array.
 *
 * @public
 * @exported
 */
export interface DataReader {
    /**
     * Indicates if the reader is at the root nesting level.
     */
    isAtRootNestingLevel: boolean;
    
    /**
     * Reads a single bit from the buffer.
     *
     * @returns {boolean} The bit value read (true for 1, false for 0).
     * @throws {Error} if attempting to read beyond the buffer length.
     */
    readBit(): boolean;

    /**
     * Reads a single byte from the buffer.
     *
     * @returns {number} The byte value read.
     */
    readByte(): number;

    /**
     * Reads an array of bytes from the data buffer.
     *
     * @param {number} size - The number of bytes to read.
     * @returns {Uint8Array} The array of bytes read from the buffer.
     */
    readBytes(size: number): Uint8Array;

    /**
     * Records the current read position of the internal storage.
     *
     * @returns {Disposable} An `Disposable` that causes the read position of the internal storage to be reset.
     */
    bookmark(): Bookmark;
}