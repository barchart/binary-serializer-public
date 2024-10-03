import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerLong } from "./binary-serializer-long";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";

/**
 * Reads (and writes) datetime values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Date>}
 */
export class BinarySerializerDateTime implements BinaryTypeSerializer<Date> {
    private binarySerializerLong: BinarySerializerLong;

    constructor() {
        this.binarySerializerLong = new BinarySerializerLong();
    }
    
    get sizeInBytes(): number {
        return 8;
    }

    encode(writer: DataWriter, value: Date): void {
        const millisecondsSinceEpoch = this.getMillisecondsSinceEpoch(value);

        this.binarySerializerLong.encode(writer, BigInt(millisecondsSinceEpoch));
    }
    
    decode(reader: DataReader): Date {
        const millisecondsSinceEpoch: bigint = this.binarySerializerLong.decode(reader);

        return new Date(Number(millisecondsSinceEpoch));
    }
    getEquals(a: Date, b: Date): boolean {
        return a.getTime() === b.getTime();
    }

    private getMillisecondsSinceEpoch(value: Date): number {
        return value.getTime();
    }
}