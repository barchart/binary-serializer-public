import { BinarySerializerInt } from "../../src";

const MAX_INT_32 = 2**31 - 1;
const MIN_INT_32 = -(2**31);

describe('BinarySerializerIntTests', () => {
    let serializer: BinarySerializerInt;

    beforeEach(() => {
        serializer = new BinarySerializerInt();
    });

    describe('Encode', () => {
        const testCases = [
            MAX_INT_32,
            MIN_INT_32,
            0,
            -1,
            1
        ];

        testCases.forEach((value) => {
            it(`should write expected bytes for ${value}`, () => {
                const writer = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bytesWritten: vi.fn(),
                    bookmark: vi.fn(),
                    isAtRootNestingLevel: true
                };
                const bitsWritten: boolean[] = [];
                const bytesWritten: number[] = [];
                const bytesArrayWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => bytesWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesArrayWritten.push(bytes));

                serializer.encode(writer, value);

                expect(bitsWritten.length).toBe(0);
                expect(bytesWritten.length).toBe(0);

                const expectedBytes = new Uint8Array(new Int32Array([value]).buffer);
                expect(bytesArrayWritten.length).toBe(1);
                expect(bytesArrayWritten[0]).toEqual(expectedBytes);
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            MAX_INT_32,
            MIN_INT_32,
            0,
            -1,
            1
        ];

        testCases.forEach((expectedValue) => {
            it(`should return expected value for ${expectedValue}`, () => {
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn(),
                    isAtRootNestingLevel: true
                };
                reader.readBytes.mockReturnValue(new Uint8Array(new Int32Array([expectedValue]).buffer));

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(expectedValue);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases = [
            [MAX_INT_32, MAX_INT_32],
            [MIN_INT_32, MIN_INT_32],
            [0, 0],
            [MAX_INT_32, MIN_INT_32],
            [MIN_INT_32, MAX_INT_32],
            [1, -1]
        ];

        testCases.forEach(([first, second]) => {
            it(`should return expected result for ${first} and ${second}`, () => {
                const actual = serializer.getEquals(first, second);
                const expected = first === second;

                expect(actual).toBe(expected);
            });
        });
    });
});