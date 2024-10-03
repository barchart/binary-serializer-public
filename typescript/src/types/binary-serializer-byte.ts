import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';
import { BinaryTypeSerializer } from './binary-type-serializer.interface';

/**
 * Reads (and writes) bytes to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<number>}
 */
export class BinarySerializerByte implements BinaryTypeSerializer<number> {
    get sizeInBytes(): number {
        return 1;
    }

    encode(writer: DataWriter, value: number): void {
        writer.writeByte(value);
    }

    decode(reader: DataReader): number {
        return reader.readByte();
    }

    getEquals(a: number, b: number): boolean {
        return a === b;
    }
}