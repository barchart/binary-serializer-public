import { DataBufferWriter, DataBufferReader, SchemaItemNested, SchemaDefinition } from "../../src";
import { Mock } from "vitest";

class TestEntity {
    nestedProperty?: TestProperty | null;
}

class TestProperty {
    propertyName?: string;
    propertyValue: number = 0;
}

interface TestSchema extends SchemaDefinition<TestProperty> {
    entityId: number;
    serialize: Mock;
    serializeChanges: Mock;
    deserialize: Mock;
    deserializeChanges: Mock;
    readHeader: Mock;
    readKey: Mock;
    getEquals: Mock;
}

describe('SchemaItemNestedTests', () => {
    let writer: DataBufferWriter;
    let reader: DataBufferReader;
    let buffer: Uint8Array;
    let schema: TestSchema;
    let schemaItemNested: SchemaItemNested<TestEntity, TestProperty>;

    beforeEach(() => {
        buffer = new Uint8Array(100000);
        writer = new DataBufferWriter(buffer);
        reader = new DataBufferReader(buffer);
        schema = {
            entityId: 1,
            serialize: vi.fn(),
            serializeChanges: vi.fn(),
            deserialize: vi.fn(),
            deserializeChanges: vi.fn(),
            readHeader: vi.fn(),
            readKey: vi.fn(),
            getEquals: vi.fn()
        };
        schemaItemNested = new SchemaItemNested<TestEntity, TestProperty>("nestedProperty", schema);
    });

    describe('Encode', () => {
        it('should call schema serialize with non-null nested property', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = new TestProperty();
            testEntity.nestedProperty.propertyName = "Test";
            testEntity.nestedProperty.propertyValue = 123;

            schemaItemNested.encode(writer, testEntity);

            expect(schema.serialize).toHaveBeenCalledWith(expect.any(DataBufferWriter), expect.objectContaining({ propertyName: "Test", propertyValue: 123 }));
        });

        it('should write null flag with null nested property', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = null;

            schemaItemNested.encode(writer, testEntity);

            expect(reader.readBit()).toBe(false);
            expect(reader.readBit()).toBe(true);
        });
    });

    describe('Decode', () => {
        it('should call schema deserialize with non-null nested property', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = new TestProperty();
            testEntity.nestedProperty.propertyName = "Test";
            testEntity.nestedProperty.propertyValue = 123;

            schema.deserializeChanges.mockImplementation((_reader: DataBufferReader, property: TestProperty) => {
                property.propertyName = "Test";
                property.propertyValue = 123;
                return property;
            });

            writer.writeBit(false);
            writer.writeBit(false);

            schemaItemNested.decode(reader, testEntity);

            expect(schema.deserializeChanges).toHaveBeenCalled();
            expect(testEntity.nestedProperty).toEqual(expect.objectContaining({ propertyName: "Test", propertyValue: 123 }));
        });

        it('should set property to null with null nested property flag', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = new TestProperty();

            writer.writeBit(false);
            writer.writeBit(true);

            schemaItemNested.decode(reader, testEntity);

            expect(testEntity.nestedProperty).toBeNull();
        });

        it('should skip deserialization with missing flag', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = new TestProperty();

            writer.writeBit(true);

            schemaItemNested.decode(reader, testEntity);

            expect(schema.deserialize).not.toHaveBeenCalled();
        });
    });

    describe('GetEquals', () => {
        it('should return true with same reference', () => {
            const testEntity = new TestEntity();
            testEntity.nestedProperty = new TestProperty();
            testEntity.nestedProperty.propertyName = "Name1";
            testEntity.nestedProperty.propertyValue = 123;

            schema.getEquals.mockReturnValue(true);

            const result = schemaItemNested.getEquals(testEntity, testEntity);

            expect(result).toBe(true);
        });

        it('should return false with different values', () => {
            const testEntity1 = new TestEntity();
            testEntity1.nestedProperty = new TestProperty();
            testEntity1.nestedProperty.propertyName = "Name1";
            testEntity1.nestedProperty.propertyValue = 123;

            const testEntity2 = new TestEntity();
            testEntity2.nestedProperty = new TestProperty();
            testEntity2.nestedProperty.propertyName = "Name2";
            testEntity2.nestedProperty.propertyValue = 456;

            schema.getEquals.mockReturnValue(false);

            const result = schemaItemNested.getEquals(testEntity1, testEntity2);

            expect(result).toBe(false);
        });

        it('should return true with same values', () => {
            const testEntity1 = new TestEntity();
            testEntity1.nestedProperty = new TestProperty();
            testEntity1.nestedProperty.propertyName = "Name1";
            testEntity1.nestedProperty.propertyValue = 123;

            const testEntity2 = new TestEntity();
            testEntity2.nestedProperty = new TestProperty();
            testEntity2.nestedProperty.propertyName = "Name1";
            testEntity2.nestedProperty.propertyValue = 123;

            schema.getEquals.mockReturnValue(true);

            const result = schemaItemNested.getEquals(testEntity1, testEntity2);

            expect(result).toBe(true);
        });
    });
});