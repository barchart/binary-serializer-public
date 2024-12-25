import { BinaryTypeSerializer, BinaryTypeSerializerBase } from "../types/binary-type-serializer.interface";
import { SchemaItemWithKeyDefinition } from "./schema-item-definition.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { DataReader } from "../buffers/data-reader.interface";
import { KeyMismatchException } from "./exceptions/key-mismatch-exception";
import { Serialization } from "../utilities/serialization";

/**
 * Information regarding a single piece of data that can be serialized
 * to a binary storage (or deserialized from binary data storage).
 *
 * @public
 * @exported
 * @implements {SchemaItemWithKeyDefinition}
 * @template TEntity - The type which contains the data to be serialized.
 * In other words, this is the source of data being serialized (or the assignment target of data being deserialized).
 * @template TMember - The type of data being serialized (which is read from the source object)
 * or deserialized (which is assigned to the source object).
 * @param {string} name - The name of the property in the source object.
 * @param {boolean} key - Indicates whether the property is a key property.
 * @param {BinaryTypeSerializerBase} serializer - The serializer that reads (and writes) the data.
 */
export class SchemaItem<TEntity, TMember> implements SchemaItemWithKeyDefinition<TEntity, TMember> {
    name: string;
    key: boolean;

    serializer: BinaryTypeSerializer<TMember>;

    constructor(name: string, key: boolean, serializer: BinaryTypeSerializerBase){
        this.name = name;
        this.key = key;

        this.serializer = serializer as BinaryTypeSerializer<TMember>;
    }

    encode(writer: DataWriter, source: TEntity): void {
        if (!this.key) {
            Serialization.writeMissingFlag(writer, false);
        }

        const member = source[this.name as keyof TEntity] as TMember;

        this.serializer.encode(writer, member);
    }

    /**
     * @throws {KeyMismatchException} - If the key property is not equal between the current and previous entities.
     */
    encodeChanges(writer: DataWriter, current: TEntity, previous: TEntity): void {
        const valuesEqual = this.getEquals(current, previous);

        if (this.key && !valuesEqual) {
            throw new KeyMismatchException(this.name, true);
        }

        if (this.key || !valuesEqual) {
            this.encode(writer, current);
        } else {
            Serialization.writeMissingFlag(writer, true);
        }
    }

    /**
     * @throws {KeyMismatchException} - If the key property is not equal between the current and previous entities.
     */
    decode(reader: DataReader, target: TEntity, existing: boolean = false): void {
        let missing = false;
        
        if (!this.key) {
            missing = Serialization.readMissingFlag(reader);
        }

        if (missing) {
            return;
        }

        const current = this.serializer.decode(reader);

        if (this.key && existing) {
            if (!this.serializer.getEquals(current, target[this.name as keyof TEntity] as TMember)) {
                throw new KeyMismatchException(this.name, false);
            }
        } else {
            target[this.name as keyof TEntity] = current as TEntity[keyof TEntity];
        }
    }

    getEquals(a: TEntity, b: TEntity): boolean {
        if (!a && !b) {
            return true;
        }

        if (a && b) {
            const first = a[this.name as keyof TEntity] as TMember;
            const second = b[this.name as keyof TEntity] as TMember;

            return this.serializer.getEquals(first, second);
        }

        return false;
    }

    read(source: TEntity): TMember {
        return source[this.name as keyof TEntity] as TMember;
    }
}