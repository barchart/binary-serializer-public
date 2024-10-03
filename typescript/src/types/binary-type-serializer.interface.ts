import { DataWriter } from "../buffers/data-writer.interface";
import { DataReader } from "../buffers/data-reader.interface";

/**
 * A marker for `BinaryTypeSerializer<T>`.
 *
 * @public
 * @exported
 */
export type BinaryTypeSerializerBase = object;

/**
 * Writes (and reads) values of type T to (and from) a binary data source.
 *
 * @public
 * @exported
 * @extends {BinaryTypeSerializerBase}
 * @template T - The type to serialize to binary (and deserialize from binary).
 */
export interface BinaryTypeSerializer<T> extends BinaryTypeSerializerBase
{
    /**
     * Writes a value to a binary data source.
     *
     * @public
     * @param {DataWriter} writer - The binary data source.
     * @param {T} value - The value to write.
     */
    encode(writer: DataWriter, value: T): void
    
     /**
     * Reads a value from a binary data source.
     *
     * @public
     * @param {DataReader} reader - The binary data source.
     * @returns {T} The value.
     */
    decode(reader: DataReader): T 

    /**
     * Indicates whether two values are equal.
     *
     * @param {T} a - The first value.
     * @param {T} b - The second value.
     * @returns {boolean} True if the two values are equal; otherwise false.
     */
    getEquals(a: T, b: T): boolean
}