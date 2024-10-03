import { SchemaFactory, BinaryTypeSerializerFactory, SchemaField, DataType, Schema } from "../../../src";

describe('SchemaFactoryTests', () => {
    let schemaFactory: SchemaFactory;
    let entityId: number;

    beforeEach(() => {
        schemaFactory = new SchemaFactory(new BinaryTypeSerializerFactory());

        entityId = 1;
    });

    it('should create a schema for primitive declarations', () => {
        const fields: SchemaField[] = [
            { name: 'id', type: DataType.int, isKey: true },
            { name: 'name', type: DataType.string }
        ];

        const schema = schemaFactory.make(entityId, fields);

        expect(schema).toBeInstanceOf(Schema);
    });

    it('should create a schema for nested objects', () => {
        const fields: SchemaField[] = [
            { name: 'id', type: DataType.int, isKey: true },
            { name: 'address', type: DataType.object, fields: [
                { name: 'street', type: DataType.string },
                { name: 'city', type: DataType.string }
            ]}
        ];

        const schema = schemaFactory.make(entityId, fields);

        expect(schema).toBeInstanceOf(Schema);
    });

    it('should create a schema for collections of objects', () => {
        const fields: SchemaField[] = [
            { name: 'id', type: DataType.int, isKey: true },
            { name: 'tags', type: DataType.list, elementType: DataType.object, fields: [
                { name: 'tag', type: DataType.string }
            ]}
        ];

        const schema = schemaFactory.make(entityId, fields);

        expect(schema).toBeInstanceOf(Schema);
    });

    it('should create a schema for collections of primitive declarations', () => {
        const fields: SchemaField[] = [
            { name: 'id', type: DataType.int, isKey: true },
            { name: 'tags', type: DataType.list, elementType: DataType.string }
        ];

        const schema = schemaFactory.make(entityId, fields);

        expect(schema).toBeInstanceOf(Schema);
    });
});