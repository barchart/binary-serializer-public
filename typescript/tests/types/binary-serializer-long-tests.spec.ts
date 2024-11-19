import { BinarySerializerLong } from "../../src";

describe('BinarySerializerLongTests', () => {
    let serializer: BinarySerializerLong;

    beforeEach(() => {
        serializer = new BinarySerializerLong();
    });

    describe('Encode', () => {
        const testCases = [
            Number.MAX_SAFE_INTEGER, 
            Number.MIN_SAFE_INTEGER, 
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
                    bookmark: vi.fn(),
                    bytesWritten: 0
                };
                const bitsWritten: boolean[] = [];
                const bytesWritten: number[] = [];
                const bytesArrayWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => bytesWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesArrayWritten.push(bytes));

                serializer.encode(writer, BigInt(value));

                expect(bitsWritten.length).toBe(0);
                expect(bytesWritten.length).toBe(0);

                const expectedBytes = new Uint8Array(new BigInt64Array([BigInt(value)]).buffer);
                expect(bytesArrayWritten.length).toBe(1);
                expect(bytesArrayWritten[0]).toEqual(expectedBytes);
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            Number.MAX_SAFE_INTEGER, 
            Number.MIN_SAFE_INTEGER, 
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
                    bytesRead: 0
                };
                reader.readBytes.mockReturnValue(new Uint8Array(new BigInt64Array([BigInt(expectedValue)]).buffer));

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(BigInt(expectedValue));
            });
        });
    });

    describe('GetEquals', () => {
        const testCases = [
            [Number.MAX_SAFE_INTEGER, Number.MAX_SAFE_INTEGER],
            [Number.MIN_SAFE_INTEGER, Number.MIN_SAFE_INTEGER],
            [0, 0],
            [Number.MAX_SAFE_INTEGER, Number.MIN_SAFE_INTEGER],
            [Number.MIN_SAFE_INTEGER, Number.MAX_SAFE_INTEGER],
            [1, -1]
        ];

        testCases.forEach(([first, second]) => {
            it(`should return expected result for ${first} and ${second}`, () => {
                const actual = serializer.getEquals(BigInt(first), BigInt(second));
                const expected = first === second;

                expect(actual).toBe(expected);
            });
        });
    });
});