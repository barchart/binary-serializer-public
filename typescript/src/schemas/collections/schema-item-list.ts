import { DataReader } from "../../buffers/data-reader.interface";
import { DataWriter } from "../../buffers/data-writer.interface";
import { SchemaDefinition } from "../schema-definition.interface";
import { SchemaItemDefinition } from "../schema-item-definition.interface";
import { Serialization } from "../../utilities/utilities";

/**
 * Manages the serialization and deserialization of list or array of complex declarations within a binary data context.
 * This class facilitates the structured encoding and decoding of item lists, leveraging a defined schema for each item
 * to ensure data integrity and support efficient data exchange. It enables both comprehensive and differential
 * serialization strategies, optimizing data storage and transmission by focusing on the differences between item states.
 *
 * @public
 * @exported
 * @implements {SchemaItemDefinition<TEntity>}
 * @template TEntity - The type which contains the data to be serialized. In other words,
 * this is the source of data being serialized (or the assignment target of data being deserialized).
 * @template TItem - The type of the items being serialized (which is read from the source
 * object) or deserialized (which is assigned to the source object).
 * @param {string} name - The name of the property in the source object.
 * @param {SchemaDefinition<TItem>} itemSchema - The schema definition for the items within the list.
 */
export class SchemaItemList<TEntity extends object, TItem extends object> implements SchemaItemDefinition<TEntity> {
    name: string;
    key: boolean;

    private readonly itemSchema: SchemaDefinition<TItem>;

    constructor(name: string, itemSchema: SchemaDefinition<TItem>) {
        this.name = name;
        this.key = false;

        this.itemSchema = itemSchema;
    }

    encode(writer: DataWriter, source: TEntity): void {
        const items = source[this.name as keyof TEntity] as Array<TItem>;

        Serialization.writeMissingFlag(writer, false);
        Serialization.writeNullFlag(writer, items === null);

        if (items === null) {
            return;
        }

        writer.writeBytes(new Uint8Array(new Uint32Array([items.length]).buffer));

        items.forEach(item => {
            if (item !== null) {
                Serialization.writeNullFlag(writer, false);

                this.itemSchema.serialize(writer, item);
            } else {
                Serialization.writeNullFlag(writer, true);
            }
        });
    }

    encodeChanges(writer: DataWriter, current: TEntity, previous: TEntity): void {
        if (this.getEquals(current, previous)) {
            Serialization.writeMissingFlag(writer, true);

            return;
        }

        const currentItems = current[this.name as keyof TEntity] as Array<TItem>;
        const previousItems = previous[this.name as keyof TEntity] as Array<TItem>  ;

        Serialization.writeMissingFlag(writer, false);
        Serialization.writeNullFlag(writer, currentItems === null);

        if (currentItems === null) {
            return;
        }

        writer.writeBytes(new Uint8Array(new Uint32Array([currentItems.length]).buffer));

        currentItems.forEach((item, index) => {
            if (currentItems[index] === null) {
                Serialization.writeNullFlag(writer, true);

                return;
            }

            Serialization.writeNullFlag(writer, false);

            if (previousItems != null && previousItems[index] !== null) {
                this.itemSchema.serializeChanges(writer, item, previousItems[index]);
            } else {
                this.itemSchema.serialize(writer, item);
            }
        });
    }

    decode(reader: DataReader, target: TEntity): void {
        if (Serialization.readMissingFlag(reader)) {
            return;
        }

        if (Serialization.readNullFlag(reader)) {
            target[this.name as keyof TEntity] = null as TEntity[keyof TEntity];

            return;
        }

        const currentItems = target[this.name as keyof TEntity] as Array<TItem>;

        const count = new Uint32Array(reader.readBytes(4).buffer)[0];
        const items: Array<TItem> = [];

        for (let i = 0; i < count; i++) {
            if (Serialization.readNullFlag(reader)) {
                items.push(null as unknown as TItem);
            } 
            else {
                if(currentItems != null && currentItems.length > i) {
                    items.push(this.itemSchema.deserializeChanges(reader, currentItems[i], true));
                } else {
                    items.push(this.itemSchema.deserialize(reader, true));
                }
            }
        }

        target[this.name as keyof TEntity] = items as TEntity[keyof TEntity];
    }

    getEquals(a: TEntity, b: TEntity): boolean {
        if (!a && !b) {
            return true;
        }

        if (!a || !b) {
            return false;
        }

        const listA = a[this.name as keyof TEntity] as Array<TItem>;
        const listB = b[this.name as keyof TEntity] as Array<TItem>;

        if (listA === null && listB === null) {
            return true;
        }

        if (listA === null || listB === null) {
            return false;
        }

        if (listA.length !== listB.length) {
            return false;
        }

        for (let i = 0; i < listA.length; i++) {
            if (!this.itemSchema.getEquals(listA[i], listB[i])) {
                return false;
            }
        }

        return true;
    }
}