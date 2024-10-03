import { DataBufferWriter, DataBufferReader, SchemaItemListPrimitive, BinarySerializerInt } from "../../../src";

class TestEntity {
    intListProperty?: number[] | null;
}

describe('SchemaItemListPrimitiveTests', () => {
    let writer: DataBufferWriter;
    let reader: DataBufferReader;
    let buffer: Uint8Array;
    let schemaItemListPrimitive: SchemaItemListPrimitive<TestEntity, number>;

    beforeEach(() => {
        buffer = new Uint8Array(100000);
        writer = new DataBufferWriter(buffer);
        reader = new DataBufferReader(buffer);

        const intSerializer = new BinarySerializerInt();
        schemaItemListPrimitive = new SchemaItemListPrimitive<TestEntity, number>("intListProperty", intSerializer);
    });

    describe('Encode', () => {
        it('should encode list with non-null property', () => {
            const testEntity = new TestEntity();
            testEntity.intListProperty = [1, 2, 3];

            schemaItemListPrimitive.encode(writer, testEntity);

            const isListNull = reader.readBit();
            const isListMissing = reader.readBit();
            const count = new DataView(reader.readBytes(4).buffer).getInt32(0, true);

            expect(isListNull).toBe(false);
            expect(isListMissing).toBe(false);
            expect(count).toBe(3);

            expect(reader.readBit()).toBe(false);
            expect(new DataView(reader.readBytes(4).buffer).getInt32(0, true)).toBe(1);
            expect(reader.readBit()).toBe(false);
            expect(new DataView(reader.readBytes(4).buffer).getInt32(0, true)).toBe(2);
            expect(reader.readBit()).toBe(false);
            expect(new DataView(reader.readBytes(4).buffer).getInt32(0, true)).toBe(3);
        });

        it('should set null flag for null list property', () => {
            const testEntity = new TestEntity();
            testEntity.intListProperty = null;

            schemaItemListPrimitive.encode(writer, testEntity);

            const isListMissing = reader.readBit();
            const isListNull = reader.readBit();

            expect(isListMissing).toBe(false);
            expect(isListNull).toBe(true);
        });
    });

    describe('Decode', () => {
        it('should set property to null for missing list property', () => {
            writer.writeBit(false);
            writer.writeBit(true);

            const testEntity = new TestEntity();

            schemaItemListPrimitive.decode(reader, testEntity);

            expect(testEntity.intListProperty).toBeNull();
        });

        it('should set property to decoded list for non-null list property', () => {
            writer.writeBit(false);
            writer.writeBit(false);

            writer.writeBytes(new Uint8Array(new Int32Array([3]).buffer));
            writer.writeBit(false);
            writer.writeBytes(new Uint8Array(new Int32Array([1]).buffer));
            writer.writeBit(false);
            writer.writeBytes(new Uint8Array(new Int32Array([2]).buffer));
            writer.writeBit(false);
            writer.writeBytes(new Uint8Array(new Int32Array([3]).buffer));
            const decodedEntity = new TestEntity();

            schemaItemListPrimitive.decode(reader, decodedEntity);

            expect(decodedEntity.intListProperty).toEqual([1, 2, 3]);
        });
    });

    describe('GetEquals', () => {
        it('should return true for identical non-null lists', () => {
            const entity1 = new TestEntity();
            entity1.intListProperty = [1, 2, 3];

            const entity2 = new TestEntity();
            entity2.intListProperty = [1, 2, 3];

            const result = schemaItemListPrimitive.getEquals(entity1, entity2);

            expect(result).toBe(true);
        });

        it('should return false for different lists', () => {
            const entity1 = new TestEntity();
            entity1.intListProperty = [1, 2, 3];

            const entity2 = new TestEntity();
            entity2.intListProperty = [4, 5, 6];

            const result = schemaItemListPrimitive.getEquals(entity1, entity2);

            expect(result).toBe(false);
        });

        it('should return true for both null lists', () => {
            const entity1 = new TestEntity();
            entity1.intListProperty = null;

            const entity2 = new TestEntity();
            entity2.intListProperty = null;

            const result = schemaItemListPrimitive.getEquals(entity1, entity2);

            expect(result).toBe(true);
        });

        it('should return false for one null list', () => {
            const entity1 = new TestEntity();
            entity1.intListProperty = null;

            const entity2 = new TestEntity();
            entity2.intListProperty = [1, 2, 3];

            const result = schemaItemListPrimitive.getEquals(entity1, entity2);

            expect(result).toBe(false);
        });
    });
});