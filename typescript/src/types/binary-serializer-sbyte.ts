import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";

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
        writer.writeByte(new Int8Array([value])[0]);
    }

    decode(reader: DataReader): number {
        const byte = reader.readByte();
        return new Int8Array([byte])[0];
    }

    getEquals(a: number, b: number): boolean {
        return a === b;
    }
}