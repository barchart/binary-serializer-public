import { BinarySerializerULong } from "../../src";

describe('BinarySerializerULongTests', () => {
    let serializer: BinarySerializerULong;

    beforeEach(() => {
        serializer = new BinarySerializerULong();
    });

    describe('Encode', () => {
        const testCases: number[] = [
            Number.MAX_SAFE_INTEGER,
            0,
            1,
            123456789
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
                const byteWritten: number[] = [];
                const bytesWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => byteWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesWritten.push(bytes));

                serializer.encode(writer, BigInt(value));

                expect(bitsWritten.length).toBe(0);
                expect(byteWritten.length).toBe(0);

                const expectedBytes = new Uint8Array(new BigUint64Array([BigInt(value)]).buffer);

                expect(bytesWritten.length).toBe(1);
                expect(bytesWritten[0].length).toBe(expectedBytes.length);

                for (let i = 0; i < expectedBytes.length; i++) {
                    expect(bytesWritten[0][i]).toBe(expectedBytes[i]);
                }
            });
        });
    });

    describe('Decode', () => {
        const testCases: number[] = [
            Number.MAX_SAFE_INTEGER,
            0,
            1,
            123456789
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
                const encodedBytes = new Uint8Array(new BigUint64Array([BigInt(expectedValue)]).buffer);

                reader.readBytes.mockReturnValue(encodedBytes);

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(BigInt(expectedValue));
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [number, number][] = [
            [Number.MAX_SAFE_INTEGER, Number.MAX_SAFE_INTEGER],
            [0, 0],
            [Number.MAX_SAFE_INTEGER, 0],
            [0, Number.MAX_SAFE_INTEGER],
            [1, 123456789]
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