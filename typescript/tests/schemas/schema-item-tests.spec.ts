import { DataBufferWriter, DataBufferReader, BinaryTypeSerializer, SchemaItem, KeyMismatchException } from '../../src';

class TestEntity {
    name: string = '';
}

describe('SchemaItemTests', () => {
    let serializerMock: BinaryTypeSerializer<string>;
    let writer: DataBufferWriter;
    let buffer: Uint8Array;

    beforeEach(() => {
        serializerMock = {
            encode: vi.fn(),
            decode: vi.fn(),
            getEquals: vi.fn()
        } as unknown as BinaryTypeSerializer<string>;
        buffer = new Uint8Array(100000);
        writer = new DataBufferWriter(buffer);
    });

    describe('Encode', () => {
        it('should write data with valid data', () => {
            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const current = new TestEntity();
            current.name = "CurrentValue";

            schemaItem.encode(writer, current);

            expect(serializerMock.encode).toHaveBeenCalledWith(expect.any(DataBufferWriter), "CurrentValue");
        });

        it('should throw KeyMismatchException with different key values', () => {
            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const current = new TestEntity();
            current.name = "CurrentValue";
            const previous = new TestEntity();
            previous.name = "PreviousValue";

            expect(() => schemaItem.encodeChanges(writer, current, previous)).toThrow(KeyMismatchException);
        });

        it('should write data with identical key values', () => {
            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const value = "SameValue";
            const current = new TestEntity();
            current.name = value;
            const previous = new TestEntity();
            previous.name = value;

            serializerMock.getEquals = vi.fn().mockReturnValue(true);

            schemaItem.encodeChanges(writer, current, previous);

            expect(serializerMock.encode).toHaveBeenCalledWith(expect.any(DataBufferWriter), value);
        });
    });

    describe('Decode', () => {
        it('should set data with valid data', () => {
            const reader = new DataBufferReader(new Uint8Array(100));
            serializerMock.decode = vi.fn().mockReturnValue("DecodedValue");

            const schemaItem = new SchemaItem<TestEntity, string>("name", false, serializerMock);
            const target = new TestEntity();

            schemaItem.decode(reader, target);

            expect(target.name).toBe("DecodedValue");
        });

        it('should throw KeyMismatchException with key and existing mismatch', () => {
            const reader = new DataBufferReader(new Uint8Array(100));
            serializerMock.decode = vi.fn().mockReturnValue("DecodedValue");
            serializerMock.getEquals = vi.fn().mockReturnValue(false);

            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const target = new TestEntity();
            target.name = "OriginalValue";

            expect(() => schemaItem.decode(reader, target, true)).toThrow(KeyMismatchException);
        });

        it('should set data with key and existing match', () => {
            const reader = new DataBufferReader(new Uint8Array(100));
            const decodedValue = "DecodedValue";
            serializerMock.decode = vi.fn().mockReturnValue(decodedValue);
            serializerMock.getEquals = vi.fn().mockReturnValue(true);

            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const target = new TestEntity();
            target.name = decodedValue;

            schemaItem.decode(reader, target, true);

            expect(target.name).toBe(decodedValue);
        });
    });

    describe('GetEquals', () => {
        it('should return true for identical values', () => {
            serializerMock.getEquals = vi.fn().mockReturnValue(true);

            const schemaItem = new SchemaItem<TestEntity, string>("name", false, serializerMock);
            const a = new TestEntity();
            a.name = "Value";
            const b = new TestEntity();
            b.name = "Value";

            const result = schemaItem.getEquals(a, b);

            expect(result).toBe(true);
        });

        it('should return false for different values', () => {
            serializerMock.getEquals = vi.fn().mockReturnValue(false);

            const schemaItem = new SchemaItem<TestEntity, string>("name", false, serializerMock);
            const a = new TestEntity();
            a.name = "ValueA";
            const b = new TestEntity();
            b.name = "ValueB";

            const result = schemaItem.getEquals(a, b);

            expect(result).toBe(false);
        });
    });

    describe('Read', () => {
        it('should return expected entity with valid data', () => {
            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const target = new TestEntity();
            target.name = "CurrentValue";

            const result = schemaItem.read(target);

            expect(result).toBe("CurrentValue");
        });

        it('should return wrong entity with wrong data', () => {
            const schemaItem = new SchemaItem<TestEntity, string>("name", true, serializerMock);
            const targetInvalid = new TestEntity();
            targetInvalid.name = "CurrentInvalid";

            const result = schemaItem.read(targetInvalid);

            expect(result).toBe("CurrentInvalid");
        });
    });
});