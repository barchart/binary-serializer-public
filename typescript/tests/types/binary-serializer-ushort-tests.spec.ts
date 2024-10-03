import { BinarySerializerUShort } from "../../src";

describe('BinarySerializerUShortTests', () => {
    let serializer: BinarySerializerUShort;

    beforeEach(() => {
        serializer = new BinarySerializerUShort();
    });

    describe('Encode', () => {
        const testCases: number[] = [
            65535,
            0,
            1,
            12345
        ];

        testCases.forEach((value) => {
            it(`should write expected bytes for ${value}`, () => {
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

                serializer.encode(writer, value);

                expect(bitsWritten.length).toBe(0);
                expect(byteWritten.length).toBe(0);

                const expectedBytes = new Uint8Array(new Uint16Array([value]).buffer);

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
            65535,
            0,
            1,
            12345
        ];

        testCases.forEach((expectedValue) => {
            it(`should return expected value for ${expectedValue}`, () => {
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn()
                };
                const encodedBytes = new Uint8Array(new Uint16Array([expectedValue]).buffer);

                reader.readBytes.mockReturnValue(encodedBytes);

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(expectedValue);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [number, number][] = [
            [65535, 65535],
            [0, 0],
            [65535, 0],
            [0, 65535],
            [1, 12345]
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