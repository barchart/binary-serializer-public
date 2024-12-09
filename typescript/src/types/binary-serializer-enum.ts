import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';
import { BinaryTypeSerializer } from './binary-type-serializer.interface';
import Enum from '@barchart/common-js/lang/Enum';

/**
 *  Reads (and writes) enumeration values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<T>}
 * @template T - The enumeration type.
 */
export class BinarySerializerEnum<T extends Enum> implements BinaryTypeSerializer<T> {
    get sizeInBytes(): number {
        return 4;
    }

    private readonly binarySerializerNumber: BinaryTypeSerializer<number>;
    private readonly enumType: new (...args: any[]) => T;

    constructor(binarySerializerNumber: BinaryTypeSerializer<number>, enumType: new (...args: any[]) => T) {
        this.binarySerializerNumber = binarySerializerNumber;
        this.enumType = enumType;
    }

    encode(writer: DataWriter, value: T): void {
        this.binarySerializerNumber.encode(writer, value.mapping as number);
    }

    decode(reader: DataReader): T {
        const intValue = this.binarySerializerNumber.decode(reader);
        return Enum.fromMapping(this.enumType, intValue) as T;
    }

    getEquals(a: T, b: T): boolean {
        if (a === b) {
            return true;
        }
        if (a === null || b === null) {
            return false;
        }
        return a.mapping === b.mapping;
    }
}