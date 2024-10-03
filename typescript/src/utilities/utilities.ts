import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";

/**
 * Provides serialization utilities.
 *
 * @public
 * @exported
 */
export class Serialization {
    /**
     * Reads a missing flag from a binary data reader.
     *
     * @param reader - The binary data reader.
     * @returns A boolean indicating whether the value is missing.
     */
    static readMissingFlag(reader: DataReader): boolean {
        return reader.readBit();
    }

    /**
     * Writes a missing flag to a binary data writer.
     *
     * @param writer - The binary data writer.
     * @param flag - A boolean indicating whether the value is missing.
     */
    static writeMissingFlag(writer: DataWriter, flag: boolean): void {
        writer.writeBit(flag);
    }

    /**
     * Reads a null flag from a binary data reader.
     *
     * @param reader - The binary data reader.
     * @returns A boolean indicating whether the value is null.
     */
    static readNullFlag(reader: DataReader): boolean {
        return reader.readBit();
    }

    /**
     * Writes a null flag to a binary data writer.
     *
     * @param writer - The binary data writer.
     * @param flag - A boolean indicating whether the value is null.
     */
    static writeNullFlag(writer: DataWriter, flag: boolean): void {
        writer.writeBit(flag);
    }
}