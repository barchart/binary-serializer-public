import { BinarySerializerNullable, DataReader, DataWriter } from "../../src";

describe('BinarySerializerNullable Tests', () => {

    describe('Encode', () => {
        it('should write the expected bits and bytes for non-null value', () => {
            const typeSerializer = {
                encode: vi.fn((writer: DataWriter) => { writer.writeByte(0xAB); }),
                decode: vi.fn(),
                getEquals: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);
            const mockWriter = {
                writeBit: vi.fn(),
                writeByte: vi.fn(),
                writeBytes: vi.fn(),
            };

            serializer.encode(mockWriter as unknown as DataWriter, true);

            expect(mockWriter.writeBit).toHaveBeenCalledWith(false);
            expect(mockWriter.writeByte).toHaveBeenCalledWith(0xAB);
        });

        it('should write a single bit when value is null', () => {
            const typeSerializer = {
                encode: vi.fn(),
                decode: vi.fn(),
                getEquals: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);
            const mockWriter = {
                writeBit: vi.fn(),
                writeByte: vi.fn(),
                writeBytes: vi.fn(),
            };

            serializer.encode(mockWriter as unknown as DataWriter, null);

            expect(mockWriter.writeBit).toHaveBeenCalledWith(true);
            expect(mockWriter.writeByte).not.toHaveBeenCalled();
        });
    });

    describe('Decode', () => {
        it('should read expected bits and bytes for non-null value', () => {
            const typeSerializer = {
                decode: vi.fn(() => 0xAB),
                encode: vi.fn(),
                getEquals: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);
            const mockReader = {
                readBit: vi.fn(() => false),
                readByte: vi.fn(() => 0xAB),
                readBytes: vi.fn(),
            };

            const result = serializer.decode(mockReader as unknown as DataReader);

            expect(result).toEqual(0xAB);
        });

        it('should return null when the first bit is true', () => {
            const typeSerializer = {
                decode: vi.fn(),
                encode: vi.fn(),
                getEquals: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);
            const mockReader = {
                readBit: vi.fn(() => true),
                readByte: vi.fn(),
                readBytes: vi.fn(),
            };

            const result = serializer.decode(mockReader as unknown as DataReader);

            expect(result).toBeNull();
        });
    });

    describe('Equals', () => {
        it('should return true for equal values', () => {
            const typeSerializer = {
                getEquals: vi.fn((a, b) => a === b),
                encode: vi.fn(),
                decode: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);

            const result = serializer.getEquals(42, 42);
            expect(result).toBe(true);
        });

        it('should return false for non-equal values', () => {
            const typeSerializer = {
                getEquals: vi.fn((a, b) => a === b),
                encode: vi.fn(),
                decode: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);

            const result = serializer.getEquals(42, 24);
            expect(result).toBe(false);
        });

        it('should return false when one value is null', () => {
            const typeSerializer = {
                getEquals: vi.fn((a, b) => a === b),
                encode: vi.fn(),
                decode: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);

            const result = serializer.getEquals(null, 42);
            expect(result).toBe(false);
        });

        it('should return true when both values are null', () => {
            const typeSerializer = {
                getEquals: vi.fn((a, b) => a === b),
                encode: vi.fn(),
                decode: vi.fn()
            };
            const serializer = new BinarySerializerNullable(typeSerializer);

            const result = serializer.getEquals(null, null);
            expect(result).toBe(true);
        });
    });
});