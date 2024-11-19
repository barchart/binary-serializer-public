import { BinarySerializerByte } from '../../src';

describe('BinarySerializerByteTests', () => {
    let serializer: BinarySerializerByte;

    beforeEach(() => {
        serializer = new BinarySerializerByte();
    });

    describe('Encode', () => {
        const testCases = [
            255,
            0,
            127,
            128
        ];

        testCases.forEach((value) => {
            it(`should write expected bytes for value ${value}`, () => {
                const mockWriter = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bookmark: vi.fn(),
                    bytesWritten: 0
                };
                const bitsWritten: boolean[] = [];
                const byteWritten: number[] = [];
                const bytesWritten: Uint8Array[] = [];

                mockWriter.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                mockWriter.writeByte.mockImplementation((byte: number) => byteWritten.push(byte));
                mockWriter.writeBytes.mockImplementation((bytes: Uint8Array) => bytesWritten.push(bytes));

                serializer.encode(mockWriter, value);

                expect(bitsWritten).toHaveLength(0);
                expect(byteWritten).toHaveLength(1);
                expect(byteWritten[0]).toBe(value);
                expect(bytesWritten).toHaveLength(0);
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            255,
            0,
            127,
            128
        ];

        testCases.forEach((value) => {
            it(`should return expected value for encoded byte ${value}`, () => {
                const mockReader = {
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    readBytes: vi.fn(),
                    bookmark: vi.fn(),
                    bytesRead: 0
                };

                mockReader.readByte.mockReturnValue(value);

                const deserialized = serializer.decode(mockReader);

                expect(deserialized).toBe(value);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [number, number][] = [
            [255, 255],
            [0, 0],
            [128, 128],
            [255, 0],
            [0, 255],
            [128, 127]
        ];

        testCases.forEach(([byte1, byte2]) => {
            it(`should match equals output for values ${byte1} and ${byte2}`, () => {
                const actual = serializer.getEquals(byte1, byte2);
                const expected = byte1 === byte2;

                expect(actual).toBe(expected);
            });
        });
    });
});