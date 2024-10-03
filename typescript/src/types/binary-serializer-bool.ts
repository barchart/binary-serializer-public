import { BinaryTypeSerializer } from './binary-type-serializer.interface';
import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';

/**
 * Reads (and writes) boolean values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<boolean>}
 */
export class BinarySerializerBool implements BinaryTypeSerializer<boolean> {
    get sizeInBytes(): number {
        return 1 / 8;
    }

    encode(writer: DataWriter, value: boolean): void {
        writer.writeBit(value);
    }

    decode(reader: DataReader): boolean {
        return reader.readBit();
    }

    getEquals(a: boolean, b: boolean): boolean {
        return a === b;
    }
}