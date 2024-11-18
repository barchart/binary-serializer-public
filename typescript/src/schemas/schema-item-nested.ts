import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { SchemaDefinition } from "./schema-definition.interface";
import { SchemaItemDefinition } from "./schema-item-definition.interface";
import { Serialization } from "../utilities/utilities";

/**
 * Provides a mechanism for handling the serialization and deserialization of nested properties within an entity,
 * facilitating the conversion to and from a binary format while managing nullability and presence flags for optimized storage efficiency.
 *
 * @public
 * @exported
 * @implements {SchemaItemDefinition<TEntity>}
 * @template TEntity - The type which contains the data to be serialized. In other words,
 * this is the source of data being serialized (or the assignment target of data being deserialized).
 * @template TMember - The type of data being serialized (which is read from the source
 * object) or deserialized (which is assigned to the source object).
 * @param {string} name - The name of the property in the source object.
 * @param {SchemaDefinition<TMember>} schema - The schema definition for the nested property.
 */
export class SchemaItemNested<TEntity extends object, TMember extends object> implements SchemaItemDefinition<TEntity> {
    name: string;
    key: boolean;

    private readonly schema: SchemaDefinition<TMember>;

    constructor(name: string, schema: SchemaDefinition<TMember>) {
        this.name = name;
        this.key = false;

        this.schema = schema;
    }

    encode(writer: DataWriter, source: TEntity): void {
        const nested = source[this.name as keyof TEntity] as TMember;

        Serialization.writeMissingFlag(writer, false);
        Serialization.writeNullFlag(writer, nested === null);

        if (nested !== null) {
            this.schema.serialize(writer, nested);
        }
    }

    encodeCompare(writer: DataWriter, current: TEntity, previous: TEntity): void {
        const unchanged = this.getEquals(current, previous);

        Serialization.writeMissingFlag(writer, unchanged);

        if (unchanged) {
            return;
        }

        const nestedCurrent = current[this.name as keyof TEntity] as TMember;
        const nestedPrevious = previous[this.name as keyof TEntity] as TMember;

        Serialization.writeNullFlag(writer, nestedCurrent === null);

        if (nestedCurrent === null) {
            return;
        }

        if (nestedPrevious === null) {
            this.schema.serialize(writer, nestedCurrent);
        } else {
            this.schema.serializeWithPrevious(writer, nestedCurrent, nestedPrevious);
        }
    }

    decode(reader: DataReader, target: TEntity): void {
        if (Serialization.readMissingFlag(reader)) {
            return;
        }

        const nestedKey = this.name as keyof TEntity;

        if (!(nestedKey in target)) {
            target[nestedKey] = null as unknown as TEntity[keyof TEntity];
        }

        const nested = target[nestedKey] as TMember;

        if (Serialization.readNullFlag(reader)) {
            if (nested !== null) {
                target[this.name as keyof TEntity] = null as TEntity[keyof TEntity];
            }
        } else if (nested === null) {
            target[this.name as keyof TEntity] = this.schema.deserialize(reader, true) as TEntity[keyof TEntity];
        } else {
            this.schema.deserializeInto(reader, nested, true);
        }
    }

    getEquals(a: TEntity, b: TEntity): boolean {
        if (!a && !b) {
            return true;
        }

        if (!a || !b) {
            return false;
        }

        return this.schema.getEquals(a[this.name as keyof TEntity] as TMember, b[this.name as keyof TEntity] as TMember);
    }
}