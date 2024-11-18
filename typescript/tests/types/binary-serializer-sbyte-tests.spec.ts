import { BinarySerializerSByte } from "../../src";

describe('BinarySerializerSByteTests', () => {
    let serializer: BinarySerializerSByte;

    beforeEach(() => {
        serializer = new BinarySerializerSByte();
    });

    describe('Encode', () => {
        const testCases: number[] = [
            127, 
            -128, 
            0, 
            -1, 
            1
        ];

        testCases.forEach((value) => {
            it(`should write expected byte for ${value}`, () => {
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
                const bytesArrayWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => byteWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesArrayWritten.push(bytes));

                serializer.encode(writer, value);

                expect(bitsWritten.length).toBe(0);
                expect(bytesArrayWritten.length).toBe(0);

                expect(byteWritten.length).toBe(1);
                expect(byteWritten[0]).toBe(value);
            });
        });
    });

    describe('Decode', () => {
        const testCases: number[] = [
            127, 
            -128, 
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
                reader.readByte.mockReturnValue(expectedValue);

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(expectedValue);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [number, number][] = [
            [127, 127],
            [-128, -128],
            [0, 0],
            [127, -128],
            [-128, 127],
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