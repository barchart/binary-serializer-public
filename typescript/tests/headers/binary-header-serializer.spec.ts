import { BinaryHeaderSerializer, InvalidHeaderException } from '../../src';

describe('BinaryHeaderSerializerTests', () => {
    let serializer: BinaryHeaderSerializer;

    beforeEach(() => {
        serializer = BinaryHeaderSerializer.Instance;
    });

    describe('Encode', () => {
        it('should return the correct byte for Encode(0, false)', () => {
            const writer = {
                writeByte: vi.fn(),
                writeBit: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bytesWritten: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            serializer.encode(writer, 0, false);

            expect(writer.writeBit).not.toHaveBeenCalled();
            expect(writer.writeByte).toHaveBeenCalledTimes(1);
            expect(writer.writeBytes).not.toHaveBeenCalled();
            expect(writer.writeByte).toHaveBeenCalledWith(0b00000000);
        });

        it('should return the correct byte for Encode(15, true)', () => {
            const writer = {
                writeByte: vi.fn(),
                writeBit: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bytesWritten: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            serializer.encode(writer, 15, true);

            expect(writer.writeBit).not.toHaveBeenCalled();
            expect(writer.writeByte).toHaveBeenCalledTimes(1);
            expect(writer.writeBytes).not.toHaveBeenCalled();
            expect(writer.writeByte).toHaveBeenCalledWith(0b10001111);
        });

        it('should throw an exception for Encode(16, true)', () => {
            const writer = {
                writeByte: vi.fn(),
                writeBit: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bytesWritten: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            expect(() => serializer.encode(writer, 16, true)).toThrow(RangeError);
        });
    });

    describe('Decode', () => {
        it('should return the expected header for Decode(0b00000000)', () => {
            const reader = {
                readByte: vi.fn().mockReturnValue(0b00000000),
                readBit: vi.fn(),
                readBytes: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            const header = serializer.decode(reader);

            expect(header.entityId).toBe(0);
            expect(header.snapshot).toBe(false);
        });

        it('should return the expected header for Decode(0b10001111)', () => {
            const reader = {
                readByte: vi.fn().mockReturnValue(0b10001111),
                readBit: vi.fn(),
                readBytes: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            const header = serializer.decode(reader);

            expect(header.entityId).toBe(15);
            expect(header.snapshot).toBe(true);
        });

        it('should throw an exception for Decode(0b10010000)', () => {
            const reader = {
                readByte: vi.fn().mockReturnValue(0b10010000),
                readBit: vi.fn(),
                readBytes: vi.fn(),
                bookmark: vi.fn(),
                isAtRootNestingLevel: true
            };

            expect(() => serializer.decode(reader)).toThrow(InvalidHeaderException);
        });
    });
});