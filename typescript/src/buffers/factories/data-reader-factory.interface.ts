import { DataReader } from "../data-reader.interface";

/**
 * A strategy for creating DataReader instances.
 *
 * @public
 * @exported
 */
export interface DataReaderFactory {
     /**
     * Creates a new DataReader instance.
     *
     * @param {Uint8Array} bytes - The binary data to read from.
     * @returns A new DataReader instance.
     */
    make(bytes: Uint8Array): DataReader;
}