import { DataType } from "../../types/data-types";
import { SchemaItem } from "../schema-item";
import { SchemaItemDefinition } from "../schema-item-definition.interface";
import { Schema } from "../schema";
import { SerializationSchemaFactory } from "./serialization-schema-factory.interface";
import { SchemaField } from "../schema-field.type";
import { BinaryTypeSerializerFactory } from '../../types/factories/binary-type-serializer-factory';
import { SchemaItemNested } from "../schema-item-nested";
import { SchemaItemList } from "../collections/schema-item-list";
import { SchemaItemListPrimitive } from "../collections/schema-item-list-primitive";
import { SchemaDefinition } from "../schema-definition.interface";

/**
 * Defines a factory for creating schemas for entities.
 *
 * @public
 * @exported
 * @implements {SerializationSchemaFactory}
 * @param {BinaryTypeSerializerFactory} binarySerializerFactory - The factory used to create binary serializers for the schema.
 */
export class SchemaFactory implements SerializationSchemaFactory {
    private readonly binaryTypeSerializerFactory: BinaryTypeSerializerFactory;

    constructor(binarySerializerFactory: BinaryTypeSerializerFactory = new BinaryTypeSerializerFactory()) {
        this.binaryTypeSerializerFactory = binarySerializerFactory;
    }
   
    make<TEntity extends object>(entityId: number = 0, fields: SchemaField[]): SchemaDefinition<TEntity> {
        const memberDataContainer: SchemaItemDefinition<TEntity>[] = fields.map(field => {
            return this.createMemberDataDefinition<TEntity>(entityId, field);
        });

        memberDataContainer.sort((a, b) => this.compareSchemaItems(a as SchemaItem<TEntity, any>, b as SchemaItem<TEntity, any>));

        return new Schema(entityId, memberDataContainer);
    }

    private createMemberDataDefinition<TEntity extends object>(entityId: number, field: SchemaField ): SchemaItemDefinition<TEntity> {
        
        if (this.isNestedClass(field) && "fields" in field) {
            const nestedSchema = this.make(entityId, field.fields);
           
            return new SchemaItemNested<TEntity, any>(field.name, nestedSchema);
        }

        if (this.isListOfObjects(field) && "fields" in field) {
            const nestedSchema = this.make(entityId, field.fields);
        
            return new SchemaItemList<TEntity, any>(field.name, nestedSchema);
        }

        if (this.isListOfPrimitiveTypes(field) && "elementType" in field) {
            const serializer = this.binaryTypeSerializerFactory.make(field.elementType);

            return new SchemaItemListPrimitive<TEntity, any>(field.name, serializer);
        }

        return this.createPrimitiveMemberData<TEntity>(field);
    }

    private createPrimitiveMemberData<TEntity>(field: SchemaField): SchemaItemDefinition<TEntity> {
        const serializer = this.binaryTypeSerializerFactory.make(field.type);
        const isKey = 'isKey' in field && field.isKey === true;

        switch (field.type) {
            case DataType.bool:
                return new SchemaItem<TEntity, boolean>(field.name, isKey, serializer);
            case DataType.char:
                return new SchemaItem<TEntity, string>(field.name, isKey, serializer);
            case DataType.dateonly:
            case DataType.datetime:
                return new SchemaItem<TEntity, Date>(field.name, isKey, serializer);
            case DataType.decimal:
            case DataType.long:
            case DataType.ulong:
                return new SchemaItem<TEntity, bigint>(field.name, isKey, serializer);
            case DataType.double:
            case DataType.float:
            case DataType.byte:
            case DataType.short:
            case DataType.int:
            case DataType.sbyte:
            case DataType.ushort:
            case DataType.uint:
            case DataType.enum:
                return new SchemaItem<TEntity, number>(field.name, isKey, serializer);
            case DataType.string:
                return new SchemaItem<TEntity, string>(field.name, isKey, serializer);
            default:
                throw new Error(`Unsupported data type: ${field.type}`);
        }
    }
  
    private isNestedClass(field: SchemaField): boolean {
        return field.type === DataType.object;
    }
    private isListOfObjects(field: SchemaField): boolean {
        return field.type === DataType.list && field.elementType == DataType.object;
    }
    private isListOfPrimitiveTypes(field: SchemaField): boolean {
        return field.type === DataType.list;
    }

    private compareSchemaItems<TEntity extends object>(a: SchemaItem<TEntity, any>, b: SchemaItem<TEntity, any>): number {
        let comparison = Number(a.key) - Number(b.key);

        if (comparison === 0) {
            comparison = a.name.localeCompare(b.name);
        }

        return comparison;
    }
}