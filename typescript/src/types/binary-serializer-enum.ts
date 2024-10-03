import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';
import { BinaryTypeSerializer } from './binary-type-serializer.interface';

/**
 *  Reads (and writes) enumeration values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<T>}
 * @template T - The enumeration type.
 */
export class BinarySerializerEnum<T extends number> implements BinaryTypeSerializer<T> {
    get sizeInBytes(): number {
        return 4;
    }

    private readonly binarySerializerInt: BinaryTypeSerializer<number>;

    constructor(binarySerializerInt: BinaryTypeSerializer<number>) {
        this.binarySerializerInt = binarySerializerInt;
    }

    encode(writer: DataWriter, value: T): void {
        this.binarySerializerInt.encode(writer, value);
    }

    decode(reader: DataReader): T {
        const intValue = this.binarySerializerInt.decode(reader);
        return intValue as T;
    }

    getEquals(a: T, b: T): boolean {
        return this.binarySerializerInt.getEquals(a, b);
    }
}