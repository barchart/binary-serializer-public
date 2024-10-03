import { BinarySerializerDateTime } from "../../src";

describe('BinarySerializerDateTimeTests', () => {
    let serializer: BinarySerializerDateTime;

    beforeEach(() => {
        serializer = new BinarySerializerDateTime();
    });

    describe('Encode', () => {
        const testCases = [
            { year: 2023, month: 1, day: 1, hour: 12, minute: 0, second: 0 },
            { year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0 },
            { year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59 },
            { year: 2000, month: 2, day: 29, hour: 15, minute: 30, second: 45 },
            { year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0 }
        ];

        testCases.forEach(({ year, month, day, hour, minute, second }) => {
            it(`should write expected bytes for ${year}-${month}-${day} ${hour}:${minute}:${second}`, () => {
                const writer = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bytesWritten: vi.fn(),
                    bookmark: vi.fn()
                };
                const bitsWritten: boolean[] = [];
                const byteWritten: number[] = [];
                const bytesWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => byteWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesWritten.push(bytes));

                const value = new Date(Date.UTC(year, month - 1, day, hour, minute, second));
                serializer.encode(writer, value);

                expect(bitsWritten.length).toBe(0);
                expect(byteWritten.length).toBe(0);

                const expectedMilliseconds = value.getTime();
                const expectedBytes = new Uint8Array(new BigInt64Array([BigInt(expectedMilliseconds)]).buffer);

                expect(bytesWritten.length).toBe(1);
                expect(bytesWritten[0]).toEqual(expectedBytes);
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            { year: 2023, month: 1, day: 1, hour: 12, minute: 0, second: 0 },
            { year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0 },
            { year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59 },
            { year: 2000, month: 2, day: 29, hour: 15, minute: 30, second: 45 },
            { year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0 }
        ];

        testCases.forEach(({ year, month, day, hour, minute, second }) => {
            it(`should return expected value for ${year}-${month}-${day} ${hour}:${minute}:${second}`, () => {
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn()
                };
                const value = new Date(Date.UTC(year, month - 1, day, hour, minute, second));
                const expectedMilliseconds = value.getTime();
                const expectedBytes = new Uint8Array(new BigInt64Array([BigInt(expectedMilliseconds)]).buffer);

                reader.readBytes.mockReturnValue(expectedBytes);

                const deserialized = serializer.decode(reader);

                expect(deserialized).toEqual(value);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases = [
            [2023, 1, 1, 12, 0, 0, 2023, 1, 1, 12, 0, 0],
            [1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0],
            [9999, 12, 31, 23, 59, 59, 9999, 12, 31, 23, 59, 59],
            [2000, 2, 29, 15, 30, 45, 2000, 2, 29, 15, 30, 45],
            [1970, 1, 1, 0, 0, 0, 1970, 1, 1, 0, 0, 1]
        ];

        testCases.forEach((dateTimes) => {
            const [year1, month1, day1, hour1, minute1, second1, year2, month2, day2, hour2, minute2, second2] = dateTimes;
            it(`should match equals output for ${year1}-${month1}-${day1} ${hour1}:${minute1}:${second1} and ${year2}-${month2}-${day2} ${hour2}:${minute2}:${second2}`, () => {
                const date1 = new Date(Date.UTC(year1, month1 - 1, day1, hour1, minute1, second1));
                const date2 = new Date(Date.UTC(year2, month2 - 1, day2, hour2, minute2, second2));

                const actual = serializer.getEquals(date1, date2);
                const expected = date1.getTime() === date2.getTime();

                expect(actual).toBe(expected);
            });
        });
    });
});