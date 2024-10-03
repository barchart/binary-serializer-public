import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerInt } from "./binary-serializer-int";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";

/**
 * Reads (and writes) dateonly values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Date>}
 */
export class BinarySerializerDateOnly implements BinaryTypeSerializer<Date> {
    private binarySerializerInt: BinarySerializerInt;

    constructor() {
        this.binarySerializerInt = new BinarySerializerInt();
    }

    get sizeInBytes(): number {
        return 4;
    }

    encode(writer: DataWriter, value: Date): void {
        this.binarySerializerInt.encode(writer, this.getDaysSinceEpoch(value));
    }

    decode(reader: DataReader): Date {
        const daysSinceEpoch = this.binarySerializerInt.decode(reader);
        return this.addDaysToDate(new Date('0001-01-01T00:00:00Z'), daysSinceEpoch);
    }

    getEquals(a: Date, b: Date): boolean {
        return a.getTime() === b.getTime();
    }

    getDaysSinceEpoch(value: Date): number {
        const epoch = new Date('0001-01-01T00:00:00Z');
        const diff = value.getTime() - epoch.getTime();
        return Math.floor(diff / (1000 * 60 * 60 * 24));
    }

    private addDaysToDate(date: Date, days: number): Date {
        const result = new Date(date);
        result.setTime(result.getTime() + days * 24 * 60 * 60 * 1000);
        return result;
    }
}