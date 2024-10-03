import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";

/**
 * Reads (and writes) ulong values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<bigint>}
 */
export class BinarySerializerULong implements BinaryTypeSerializer<bigint> {
    get sizeInBytes(): number {
        return 8;
    }

    encode(writer: DataWriter, value: bigint): void {
        const buffer = new ArrayBuffer(this.sizeInBytes);
        const view = new DataView(buffer);
        view.setBigUint64(0, value, true);
        
        writer.writeBytes(new Uint8Array(buffer));
    }

    decode(reader: DataReader): bigint {
        const valueBytes = reader.readBytes(this.sizeInBytes);

        return new DataView(valueBytes.buffer, valueBytes.byteOffset, valueBytes.byteLength).getBigUint64(0, true);
    }

    getEquals(a: bigint, b: bigint): boolean {
        return a === b;
    }
}