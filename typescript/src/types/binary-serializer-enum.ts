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

    private readonly binarySerializerInt: BinaryTypeSerializer<number>;
    private readonly enumType: new (...args: any[]) => T;

    constructor(binarySerializerInt: BinaryTypeSerializer<number>, enumType: new (...args: any[]) => T) {
        this.binarySerializerInt = binarySerializerInt;
        this.enumType = enumType;
    }

    encode(writer: DataWriter, value: T): void {
        this.binarySerializerInt.encode(writer, value.mapping as number);
    }

    decode(reader: DataReader): T {
        const intValue = this.binarySerializerInt.decode(reader);
        return Enum.fromMapping(this.enumType, intValue) as T;
    }

    getEquals(a: T, b: T): boolean {
        return this.binarySerializerInt.getEquals(a.mapping as number, b.mapping as number);
    }
}