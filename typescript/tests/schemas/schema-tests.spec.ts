import { Schema, SchemaItem, BinarySerializerString, KeyUndefinedException, Header, DataBufferWriter, DataBufferReader } from '../../src';

class TestEntity {
    keyProperty: string = '';
    valueProperty: string = '';
}

describe('SchemaTests', () => {
    let schema: Schema<TestEntity>;
    const entityId = 1;

    beforeEach(() => {
        const serializer = new BinarySerializerString();

        const keyItem = new SchemaItem<TestEntity, string>(
            'keyProperty',
            true,
            serializer
        );

        const valueItem = new SchemaItem<TestEntity, string>(
            'valueProperty',
            false,
            serializer
        );

        schema = new Schema<TestEntity>(entityId, [keyItem, valueItem]);
    });

    describe('Serialize', () => {
        it('should write data correctly with valid data', () => {
            const entity = new TestEntity();
            entity.keyProperty = 'Key';
            entity.valueProperty = 'Value';

            const writer: DataBufferWriter = new DataBufferWriter(new Uint8Array(100));

            const bytes = schema.serialize(writer, entity);

            expect(bytes).not.toHaveLength(0);
        });
    });

    describe('Deserialize', () => {
        it('should read data correctly with valid data', () => {
            const entity = new TestEntity();
            entity.keyProperty = 'Key';
            entity.valueProperty = 'Value';

            const writer: DataBufferWriter = new DataBufferWriter(new Uint8Array(100));
            const bytes = schema.serialize(writer, entity);
            const reader: DataBufferReader = new DataBufferReader(bytes);

            const result = schema.deserialize(reader);

            expect(result).not.toBeNull();
        });
    });

    describe('Read Header', () => {
        it('should return the correct header when called', () => {
            const entity = new TestEntity();
            entity.keyProperty = 'Key';
            entity.valueProperty = 'Value';

            const writer: DataBufferWriter = new DataBufferWriter(new Uint8Array(100));
            const bytes = schema.serialize(writer, entity);
            const reader: DataBufferReader = new DataBufferReader(bytes);

            const expectedHeader = new Header(entityId, true);
            const header = schema.readHeader(reader);

            expect(header).toEqual(expectedHeader);
        });
    });

    describe('ReadKey', () => {
        it('should return the expected value with a valid key', () => {
            const expectedValue = 'Key';

            const entity = new TestEntity();
            entity.keyProperty = 'Key';
            entity.valueProperty = 'Value';

            const writer: DataBufferWriter = new DataBufferWriter(new Uint8Array(100));
            const bytes = schema.serialize(writer, entity);
            const reader: DataBufferReader = new DataBufferReader(bytes);

            const actualValue = schema.readKey<string>(reader, 'keyProperty');

            expect(actualValue).toEqual(expectedValue);
        });

        it('should throw KeyUndefinedException with an invalid key', () => {
            const entity = new TestEntity();
            entity.keyProperty = 'Key';
            entity.valueProperty = 'Value';

            const writer: DataBufferWriter = new DataBufferWriter(new Uint8Array(100));
            const bytes = schema.serialize(writer, entity);
            const reader: DataBufferReader = new DataBufferReader(bytes);

            expect(() => schema.readKey<string>(reader, 'invalidKey')).toThrow(KeyUndefinedException);
        });
    });

    describe('GetEquals', () => {
        it('should return true for equal entities', () => {
            const entityA = new TestEntity();
            entityA.keyProperty = 'value';
            entityA.valueProperty = 'value';

            const entityB = new TestEntity();
            entityB.keyProperty = 'value';
            entityB.valueProperty = 'value';

            const result = schema.getEquals(entityA, entityB);

            expect(result).toBe(true);
        });

        it('should return false for different entities', () => {
            const entityA = new TestEntity();
            entityA.keyProperty = 'valueA';
            entityA.valueProperty = 'value';

            const entityB = new TestEntity();
            entityB.keyProperty = 'valueB';
            entityB.valueProperty = 'value';

            const result = schema.getEquals(entityA, entityB);

            expect(result).toBe(false);
        });
    });
});