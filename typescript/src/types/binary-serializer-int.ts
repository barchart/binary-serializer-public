import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import { assertIntegerInRange } from './validation';

const MINIMUM_VALUE = -0x80000000;
const MAXIMUM_VALUE = 0x7FFFFFFF;

/**
 * Reads (and writes) int values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<number>}
 */
export class BinarySerializerInt implements BinaryTypeSerializer<number> {
    get sizeInBytes(): number {
        return 4;
    }

    encode(writer: DataWriter, value: number): void {
        assertIntegerInRange(value, MINIMUM_VALUE, MAXIMUM_VALUE);

        const buffer = new ArrayBuffer(this.sizeInBytes);
        const view = new DataView(buffer);

        view.setInt32(0, value, true);

        writer.writeBytes(new Uint8Array(buffer));
    }

    decode(reader: DataReader): number {
        const valueBytes = reader.readBytes(this.sizeInBytes);

        return new DataView(valueBytes.buffer, valueBytes.byteOffset, valueBytes.byteLength).getInt32(0, true);
    }

    getEquals(a: number, b: number): boolean {
        return a === b;
    }
}
