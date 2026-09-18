import { SchemaFactory, BinaryTypeSerializerFactory, SchemaField, DataType, Schema, DataBufferReader, DataBufferWriter } from "../../../src";
import Enum from '@barchart/common-js/lang/Enum';

class TestEnum extends Enum {
    static Value = new TestEnum('Value', 'Value', 1);

    constructor(code: string, description: string, mapping: number) {
        super(code, description, mapping);
    }
}

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

    it('should sort schema fields using case-insensitive ordinal comparison', () => {
        const fields: SchemaField[] = [
            { name: '_value', type: DataType.int },
            { name: 'zValue', type: DataType.int }
        ];
     
        const schema = schemaFactory.make<{ _value: number; zValue: number }>(entityId, fields);
        
        const writer = new DataBufferWriter(new Uint8Array(100));
        const reader = new DataBufferReader(schema.serialize(writer, { _value: 1, zValue: 2 }));

        schema.readHeader(reader);
        reader.readBit();

        expect(new DataView(reader.readBytes(4).buffer).getInt32(0, true)).toBe(2);
    });

    it('should serialize nullable primitive list elements', () => {
        const fields: SchemaField[] = [
            { name: 'id', type: DataType.int, isKey: true },
            { name: 'values', type: DataType.list, elementType: DataType.int, nullable: true }
        ];
       
        const schema = schemaFactory.make<{ id: number; values: Array<number | null> }>(entityId, fields);
      
        const source = { id: 1, values: [ 1, null, 2 ] };
       
        const writer = new DataBufferWriter(new Uint8Array(100));
        const reader = new DataBufferReader(schema.serialize(writer, source));

        expect(schema.deserialize(reader)).toEqual(source);
    });

    it('should pass the underlying type for an enum field', () => {
        const serializerFactory = new BinaryTypeSerializerFactory();
        
        const make = vi.spyOn(serializerFactory, 'make');
       
        const factory = new SchemaFactory(serializerFactory);
        const fields: SchemaField[] = [
            { name: 'value', type: DataType.enum, enumType: TestEnum, enumUnderlyingType: DataType.byte }
        ];

        factory.make(entityId, fields);

        expect(make).toHaveBeenCalledWith(DataType.enum, TestEnum, DataType.byte);
    });

    it('should pass the underlying type for a nullable enum field', () => {
        const serializerFactory = new BinaryTypeSerializerFactory();
        const makeNullable = vi.spyOn(serializerFactory, 'makeNullable');
       
        const factory = new SchemaFactory(serializerFactory);
        const fields: SchemaField[] = [
            { name: 'value', type: DataType.enum, enumType: TestEnum, enumUnderlyingType: DataType.byte, nullable: true }
        ];

        factory.make(entityId, fields);

        expect(makeNullable).toHaveBeenCalledWith(DataType.enum, TestEnum, DataType.byte);
    });

    it('should pass the underlying type for enum list elements', () => {
        const serializerFactory = new BinaryTypeSerializerFactory();
        const make = vi.spyOn(serializerFactory, 'make');
      
        const factory = new SchemaFactory(serializerFactory);
        const fields: SchemaField[] = [
            { name: 'values', type: DataType.list, elementType: DataType.enum, enumType: TestEnum, enumUnderlyingType: DataType.byte }
        ];

        factory.make(entityId, fields);

        expect(make).toHaveBeenCalledWith(DataType.enum, TestEnum, DataType.byte);
    });

    it('should pass the underlying type for nullable enum list elements', () => {
        const serializerFactory = new BinaryTypeSerializerFactory();
        const makeNullable = vi.spyOn(serializerFactory, 'makeNullable');
        
        const factory = new SchemaFactory(serializerFactory);
        const fields: SchemaField[] = [
            { name: 'values', type: DataType.list, elementType: DataType.enum, enumType: TestEnum, enumUnderlyingType: DataType.byte, nullable: true }
        ];

        factory.make(entityId, fields);

        expect(makeNullable).toHaveBeenCalledWith(DataType.enum, TestEnum, DataType.byte);
    });
});
