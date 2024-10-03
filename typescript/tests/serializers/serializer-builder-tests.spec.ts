import { SerializerBuilder, Serializer, SchemaFactory, BinaryTypeSerializerFactory,
    DataBufferWriterFactory, DataBufferReaderFactory, SchemaField, DataType } from '../../src';

class TestEntity {
    keyProperty: string = "";
    valueProperty: string = "";
}

describe('SerializerBuilderTests', () => {
    let serializerBuilder: SerializerBuilder<TestEntity>;
    const entityId = 1;

    beforeEach(() => {
        serializerBuilder = new SerializerBuilder<TestEntity>(entityId);
    });

    describe('WithSchemaFactory', () => {
        it('should set schema factory', () => {
            const fields: SchemaField[] = [
                { name: 'KeyProperty', type: DataType.string, isKey: true },
                { name: 'ValueProperty', type: DataType.string, isKey: false }
            ];

            const schemaFactory = new SchemaFactory();

            serializerBuilder.withSchemaFactory(schemaFactory);
            const serializer = serializerBuilder.build(fields);

            expect(serializer).not.toBeNull();
            expect(serializer).toBeInstanceOf(Serializer);
        });

        it('should set schema factory with type serializer factory', () => {
            const fields: SchemaField[] = [
                { name: 'KeyProperty', type: DataType.string, isKey: true },
                { name: 'ValueProperty', type: DataType.string, isKey: false }
            ];

            const typeSerializerFactory = new BinaryTypeSerializerFactory();

            serializerBuilder.withSchemaFactoryUsingBinaryTypeSerializerFactory(typeSerializerFactory);
            const serializer = serializerBuilder.build(fields);

            expect(serializer).not.toBeNull();
        });
    });

    describe('WithDataBufferReaderFactory', () => {
        it('should set data buffer reader factory', () => {
            const fields: SchemaField[] = [
                { name: 'KeyProperty', type: DataType.string, isKey: true },
                { name: 'ValueProperty', type: DataType.string, isKey: false }
            ];

            const dataBufferReaderFactory = new DataBufferReaderFactory();

            serializerBuilder.withDataBufferReaderFactory(dataBufferReaderFactory);
            const serializer = serializerBuilder.build(fields);

            expect(serializer).not.toBeNull();
        });
    });

    describe('WithDataBufferWriterFactory', () => {
        it('should set data buffer writer factory', () => {
            const fields: SchemaField[] = [
                { name: 'KeyProperty', type: DataType.string, isKey: true },
                { name: 'ValueProperty', type: DataType.string, isKey: false }
            ];

            const dataBufferWriterFactory = new DataBufferWriterFactory();

            serializerBuilder.withDataBufferWriterFactory(dataBufferWriterFactory);
            const serializer = serializerBuilder.build(fields);

            expect(serializer).not.toBeNull();
        });
    });

    describe('Build', () => {
        it('should return an instance of Serializer', () => {
            const fields: SchemaField[] = [
                { name: 'KeyProperty', type: DataType.string, isKey: true },
                { name: 'ValueProperty', type: DataType.string, isKey: false }
            ];

            const serializer = serializerBuilder.build(fields);

            expect(serializer).not.toBeNull();
            expect(serializer).toBeInstanceOf(Serializer);
        });
    });

    describe('ForType', () => {
        it('should return a new SerializerBuilder instance', () => {
            const builder = SerializerBuilder.forType<TestEntity>();

            expect(builder).not.toBeNull();
            expect(builder).toBeInstanceOf(SerializerBuilder);
        });
    });
});