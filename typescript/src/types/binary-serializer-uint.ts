import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";

/**
 * Reads (and writes) uint values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer}
 */
export class BinarySerializerUInt implements BinaryTypeSerializer<number> {
    get sizeInBytes(): number {
        return 4;
    }

    encode(writer: DataWriter, value: number): void {
        const buffer = new ArrayBuffer(this.sizeInBytes);
        const view = new DataView(buffer);
        view.setUint32(0, value, true);
        
        writer.writeBytes(new Uint8Array(buffer));
    }

    decode(reader: DataReader): number {
        const valueBytes = reader.readBytes(this.sizeInBytes);

        return new DataView(valueBytes.buffer, valueBytes.byteOffset, valueBytes.byteLength).getUint32(0, true);
    }

    getEquals(a: number, b: number): boolean {
        return a === b;
    }

}