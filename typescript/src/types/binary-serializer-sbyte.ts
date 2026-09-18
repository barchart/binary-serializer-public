import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import { assertIntegerInRange } from './validation';

const MINIMUM_VALUE = -0x80;
const MAXIMUM_VALUE = 0x7F;
const BYTE_MASK = 0xFF;

/**
 * Reads (and writes) sbyte values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<number>}
 */
export class BinarySerializerSByte implements BinaryTypeSerializer<number> {
    get sizeInBytes(): number {
        return 1;
    }

    encode(writer: DataWriter, value: number): void {
        assertIntegerInRange(value, MINIMUM_VALUE, MAXIMUM_VALUE);

        writer.writeByte(value & BYTE_MASK);
    }

    decode(reader: DataReader): number {
        const byte = reader.readByte();

        return new Int8Array([byte])[0];
    }

    getEquals(a: number, b: number): boolean {
        return a === b;
    }
}
