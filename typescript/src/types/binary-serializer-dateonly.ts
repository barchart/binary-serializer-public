import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerInt } from "./binary-serializer-int";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import Day from "@barchart/common-js/lang/Day";

/**
 * Reads (and writes) dateonly values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Day>}
 */
export class BinarySerializerDateOnly implements BinaryTypeSerializer<Day> {
    private binarySerializerInt: BinarySerializerInt;

    constructor() {
        this.binarySerializerInt = new BinarySerializerInt();
    }

    get sizeInBytes(): number {
        return 4;
    }

    encode(writer: DataWriter, value: Day): void {
        this.binarySerializerInt.encode(writer, this.getDaysSinceEpoch(value));
    }

    decode(reader: DataReader): Day {
        const daysSinceEpoch = this.binarySerializerInt.decode(reader);
        return this.addDaysToEpoch(daysSinceEpoch);
    }

    getEquals(a: Day, b: Day): boolean {
        return a.getIsEqual(b);
    }

    getDaysSinceEpoch(value: Day): number {
        const epoch = new Day(1, 1, 1);
        return Day.countDaysBetween(epoch, value);
    }

    private addDaysToEpoch(days: number): Day {
        const epoch = new Day(1, 1, 1);
        return epoch.addDays(days);
    }
}