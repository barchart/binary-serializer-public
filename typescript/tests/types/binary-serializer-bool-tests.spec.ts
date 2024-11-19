import { BinarySerializerBool } from '../../src';

describe('BinarySerializerBoolTests', () => {
    let serializer: BinarySerializerBool;

    beforeEach(() => {
        serializer = new BinarySerializerBool();
    });

    describe('Encode', () => {
        it('should write true to the data buffer', () => {
            const writer = {
                writeBit: vi.fn(),
                writeByte: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bookmark: vi.fn(),
                bytesWritten: 0
            };
            const bitsWritten: boolean[] = [];

            writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));

            serializer.encode(writer, true);

            expect(bitsWritten.length).toBe(1);
            expect(bitsWritten[0]).toBe(true);

            expect(writer.writeByte).not.toHaveBeenCalled();
            expect(writer.writeBytes).not.toHaveBeenCalled();
        });

        it('should write false to the data buffer', () => {
            const mockWriter = {
                writeBit: vi.fn(),
                writeByte: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bookmark: vi.fn(),
                bytesWritten: 0
            };
            const bitsWritten: boolean[] = [];

            mockWriter.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));

            serializer.encode(mockWriter, false);

            expect(bitsWritten.length).toBe(1);
            expect(bitsWritten[0]).toBe(false);

            expect(mockWriter.writeByte).not.toHaveBeenCalled();
            expect(mockWriter.writeBytes).not.toHaveBeenCalled();
        });
    });

    describe('Decode', () => {
        it('should return true for serialized true value', () => {
            const reader = {
                readBit: vi.fn(),
                readByte: vi.fn(),
                readBytes: vi.fn(),
                bookmark: vi.fn(),
                bytesRead: 0
            };

            reader.readBit.mockReturnValue(true);

            const deserialized = serializer.decode(reader);

            expect(deserialized).toBe(true);
        });

        it('should return false for serialized false value', () => {
            const mockReader = {
                readBit: vi.fn(),
                readByte: vi.fn(),
                readBytes: vi.fn(),
                bookmark: vi.fn(),
                bytesRead: 0
            };

            mockReader.readBit.mockReturnValue(false);

            const deserialized = serializer.decode(mockReader);

            expect(deserialized).toBe(false);
        });
    });

    describe('GetEquals', () => {
        const testCases: [boolean, boolean][] = [
            [true, true],
            [false, false],
            [false, true],
            [true, false]
        ];

        testCases.forEach(([bool1, bool2]) => {
            it(`should match equals output for ${bool1} and ${bool2}`, () => {
                const actual = serializer.getEquals(bool1, bool2);
                const expected = bool1 === bool2;

                expect(actual).toBe(expected);
            });
        });
    });
});