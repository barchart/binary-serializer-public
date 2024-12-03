import { BinarySerializerDateOnly } from "../../src";
import Day from "@barchart/common-js/lang/Day";

describe('BinarySerializerDateOnlyTests', () => {
    let serializer: BinarySerializerDateOnly;

    beforeEach(() => {
        serializer = new BinarySerializerDateOnly();
    });

    describe('Encode', () => {
        const testCases = [
            { year: 2023, month: 1, day: 1 },
            { year: 1, month: 1, day: 1 },
            { year: 9999, month: 12, day: 31 },
            { year: 2000, month: 2, day: 29 },
            { year: 1970, month: 1, day: 1 }
        ];

        testCases.forEach(({ year, month, day }) => {
            it(`should write expected bytes for ${year}-${month}-${day}`, () => {
                const writer = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bookmark: vi.fn(),
                    bytesWritten: 0
                };
                const value = new Day(year, month, day);

                serializer.encode(writer, value);

                const daysSinceEpoch = serializer.getDaysSinceEpoch(value);
                const expectedBytes = new Uint8Array(new Int32Array([daysSinceEpoch]).buffer);

                expect(writer.writeBytes).toHaveBeenCalledWith(expectedBytes);
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            { year: 2023, month: 1, day: 1 },
            { year: 1, month: 1, day: 1 },
            { year: 9999, month: 12, day: 31 },
            { year: 2000, month: 2, day: 29 },
            { year: 1970, month: 1, day: 1 }
        ];

        testCases.forEach(({ year, month, day }) => {
            it(`should return expected value for ${year}-${month}-${day}`, () => {
                const value = new Day(year, month, day);
                const daysSinceEpoch = serializer.getDaysSinceEpoch(value);
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn(),
                    bytesRead: 0
                };
                reader.readBytes.mockReturnValue(new Uint8Array(new Int32Array([daysSinceEpoch]).buffer));

                const deserialized = serializer.decode(reader);

                expect(deserialized).toEqual(value);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases = [
            [2023, 1, 1, 2023, 1, 1],
            [2023, 1, 1, 2023, 1, 2],
            [1, 1, 1, 9999, 12, 31],
            [2000, 2, 29, 2000, 2, 29],
            [1970, 1, 1, 1970, 1, 1]
        ];

        testCases.forEach(([year1, month1, day1, year2, month2, day2]) => {
            it(`should match equals output for ${year1}-${month1}-${day1} and ${year2}-${month2}-${day2}`, () => {
                const date1 = new Day(year1, month1, day1);
                const date2 = new Day(year2, month2, day2);

                const actual = serializer.getEquals(date1, date2);
                const expected = date1.day === date2.day && date1.month === date2.month && date1.year === date2.year;

                expect(actual).toBe(expected);
            });
        });
    });
});