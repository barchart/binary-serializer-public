import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import * as is from '@barchart/common-js/lang/is';

const NAN_BITS = 0xFFC00000;

/**
 * Reads (and writes) float values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<number>}
 */
export class BinarySerializerFloat implements BinaryTypeSerializer<number> {
    get sizeInBytes(): number {
        return 4;
    }

    encode(writer: DataWriter, value: number): void {
        const buffer = new ArrayBuffer(this.sizeInBytes);

        const view = new DataView(buffer);

        if (is.nan(value)) {
            view.setUint32(0, NAN_BITS, true);
        } else {
            view.setFloat32(0, value, true);
        }

        writer.writeBytes(new Uint8Array(buffer));
    }

    decode(reader: DataReader): number {
        const valueBytes = reader.readBytes(this.sizeInBytes);

        return new DataView(valueBytes.buffer, valueBytes.byteOffset, valueBytes.byteLength).getFloat32(0, true);
    }

    getEquals(a: number, b: number): boolean {
        const floatA = Math.fround(a);
        const floatB = Math.fround(b);

        return floatA === floatB || (is.nan(floatA) && is.nan(floatB));
    }
}
