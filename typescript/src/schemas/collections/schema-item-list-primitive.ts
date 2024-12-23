import { DataReader } from "../../buffers/data-reader.interface";
import { DataWriter } from "../../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "../../types/binary-type-serializer.interface";
import { SchemaItemDefinition } from "../schema-item-definition.interface";
import { Serialization } from "../../utilities/serialization";

/**
 * Manages the serialization and deserialization of a list or array of primitive declarations associated with an entity
 * into a binary format. This class abstracts the complexities of handling binary data conversion for lists,
 * including support for both complete and differential serialization to efficiently manage data changes.
 * It utilizes a specified serializer for the elements within the list to ensure accurate type handling.
 *
 * @public
 * @exported
 * @implements {SchemaItemDefinition<TEntity>}
 * @template TEntity - The type which contains the data to be serialized. In other words,
 * this is the source of data being serialized (or the assignment target of data being deserialized).
 * @template TItem - The type of the items being serialized (which is read from the source
 * object) or deserialized (which is assigned to the source object).
 * @param {string} name - The name of the property in the source object.
 * @param {BinaryTypeSerializer<TItem>} elementSerializer - The serializer for the elements within the list.
 */
export class SchemaItemListPrimitive<TEntity, TItem> implements SchemaItemDefinition<TEntity> {
    name: string;
    key: boolean;

    private readonly elementSerializer: BinaryTypeSerializer<TItem>;

    constructor(name: string, elementSerializer: BinaryTypeSerializer<TItem>) {
        this.name = name;
        this.key = false;

        this.elementSerializer = elementSerializer;
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
            Serialization.writeMissingFlag(writer, false);

            this.elementSerializer.encode(writer, item);
        });
    }

    encodeChanges(writer: DataWriter, current: TEntity, previous: TEntity): void {
        if (this.getEquals(current, previous)) {
            Serialization.writeMissingFlag(writer, true);

            return;
        }

        const currentItems = current[this.name as keyof TEntity] as Array<TItem>;
        const previousItems = previous[this.name as keyof TEntity] as Array<TItem>;

        Serialization.writeMissingFlag(writer, false);
        Serialization.writeNullFlag(writer, currentItems === null);

        if (currentItems !== null) {
            this.writeItems(writer, currentItems, previousItems);
        }
    }

    decode(reader: DataReader, target: TEntity): void {
        if (Serialization.readMissingFlag(reader)) {
            return;
        }

        const currentItems = target[this.name as keyof TEntity] as Array<TItem>;

        if (Serialization.readNullFlag(reader)) {
            if (currentItems !== null) {
                target[this.name as keyof TEntity] = null as TEntity[keyof TEntity];
            }

            return;
        }

        const itemCount = new DataView(reader.readBytes(4).buffer).getUint32(0, true);
        const items: TItem[] = [];

        for (let i = 0; i < itemCount; i++) {
            if (Serialization.readMissingFlag(reader)) {
                items.push(currentItems[i]);
            } else {
                const decodedItem = this.elementSerializer.decode(reader);

                if (items.length > i){
                    items[i] = decodedItem;
                } else {
                    items.push(decodedItem);
                }
            }
        }

        target[this.name as keyof TEntity] = items as TEntity[keyof TEntity];
    }

    applyChanges(target: TEntity, source: TEntity): void {
        if (source == null) {
            return;
        }

        const sourceItems: TItem[] = source[this.name as keyof TEntity] as TItem[];

        if (sourceItems == null) {
            return;
        }

        if (target == null) {
            target = {} as TEntity;
        }

        target[this.name as keyof TEntity] = sourceItems as unknown as TEntity[keyof TEntity];
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
            if (!this.elementSerializer.getEquals(listA[i], listB[i])) {
                return false;
            }
        }

        return true;
    }

    private writeItems(writer: DataWriter, currentItems: Array<TItem>, previousItems?: Array<TItem>): void {
        writer.writeBytes(new Uint8Array(new Uint32Array([currentItems.length]).buffer));

        currentItems.forEach((item, index) => {
            const previousItem = previousItems && previousItems.length > index ? previousItems[index] : undefined;
            this.writeItem(writer, item, previousItem);
        });
    }

    private writeItem(writer: DataWriter, currentItem: TItem, previousItem?: TItem): void {
        if (currentItem !== undefined && previousItem !== undefined && this.elementSerializer.getEquals(currentItem, previousItem)) {
            Serialization.writeMissingFlag(writer, true);
        } else {
            Serialization.writeMissingFlag(writer, false);
            this.elementSerializer.encode(writer, currentItem);
        }
    }
}