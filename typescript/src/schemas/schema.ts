import { SchemaDefinition } from "./schema-definition.interface";
import { BinaryHeaderSerializer } from "../headers/binary-header-serializer";
import { SchemaItemDefinition, SchemaItemWithKeyDefinition } from "./schema-item-definition.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { DataReader } from "../buffers/data-reader.interface";
import { Header } from "../headers/header";
import { HeaderMismatchException } from "./exceptions/header-mismatch-exception";
import { KeyUndefinedException } from "./exceptions/key-undefined-exception";
import { Bookmark } from "../buffers/data-buffer-reader";

/**
 * Serializes and deserializes instances of the TEntity class.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity that can be serialized or deserialized.
 * @implements {SchemaDefinition}
 * @param {SchemaItemDefinition<TEntity>[]} items - The items of entity that make up the schema.
 * @param {number} entityId - The entity ID to be included in the header. This ID helps
 * identify the type of entity the data represents.
 */
export class Schema<TEntity extends object> implements SchemaDefinition<TEntity> {
    private readonly keyItems: SchemaItemDefinition<TEntity>[];
    private readonly valueItems: SchemaItemDefinition<TEntity>[];

    private headerSerializer: BinaryHeaderSerializer;

    public readonly entityId: number;

    constructor(items: SchemaItemDefinition<TEntity>[]);
    constructor(entityId: number, items: SchemaItemDefinition<TEntity>[]);
    constructor(entityIdOrItems: number | SchemaItemDefinition<TEntity>[], items?: SchemaItemDefinition<TEntity>[]) {
        if (typeof entityIdOrItems === 'number') {
            this.entityId = entityIdOrItems;
            items = items!;
        } else {
            this.entityId = 0;
            items = entityIdOrItems;
        }

        this.keyItems = items.filter(i => i.key);
        this.valueItems = items.filter(i => !i.key);

        this.headerSerializer = BinaryHeaderSerializer.Instance;
    }

    serialize(writer: DataWriter, source: TEntity): Uint8Array {
        if (source === null) {
            throw new Error("source cannot be null");
        }

        if (writer.bytesWritten === 0) {
            this.headerSerializer.encode(writer, this.entityId, true);
        }

        this.keyItems.forEach(item => {
            item.encode(writer, source);
        });

        this.valueItems.forEach(item => {
            item.encode(writer, source);
        });

        return writer.toBytes();
    }

    serializeChanges(writer: DataWriter, current: TEntity, previous: TEntity): Uint8Array {
        if (previous === null) {
            return this.serialize(writer, current);
        }

        if (writer.bytesWritten === 0) {
            this.headerSerializer.encode(writer, this.entityId, false);
        }

        this.keyItems.forEach(item => {
            item.encodeChanges(writer, current, previous);
        });

        this.valueItems.forEach(item => {
            item.encodeChanges(writer, current, previous);
        });

        return writer.toBytes();
    }

    deserialize(reader: DataReader): TEntity {
        return this.deserializeChanges(reader, {} as TEntity, false);
    }

    deserializeChanges(reader: DataReader, target: TEntity, existing: boolean = true): TEntity {
        if (target === null) {
            throw new Error("target cannot be null");
        }

        if (reader.bytesRead === 0) {
            const header = this.readHeader(reader);
            this.checkHeader(header);
        }

        this.keyItems.forEach(item => {
            item.decode(reader, target, existing);
        });

        this.valueItems.forEach(item => {
            item.decode(reader, target, existing);
        });

        return target;
    }

    readHeader(reader: DataReader): Header {
        return this.headerSerializer.decode(reader);
    }

    readKey<TMember>(reader: DataReader, name: string): TMember {
        const bookmark: Bookmark = reader.bookmark();

        const header = this.readHeader(reader);
        this.checkHeader(header);

        const target = {} as TEntity;

        for (const candidate of this.keyItems) {
            candidate.decode(reader, target, false);

            if (candidate.name === name && 'read' in candidate) {
                return (candidate as SchemaItemWithKeyDefinition<TEntity, TMember>).read(target);
            }
        }

        bookmark.dispose();
        throw new KeyUndefinedException(name);
    }

    getEquals(a: TEntity, b: TEntity): boolean {
        if (!a && !b) {
            return true;
        }

        if (a && b) {
            return this.keyItems.every(si => si.getEquals(a, b)) && this.valueItems.every(si => si.getEquals(a, b));
        }

        return false;
    }

    private checkHeader(header: Header): void {
        if (header.entityId !== this.entityId) {
            throw new HeaderMismatchException(header.entityId, this.entityId);
        }
    }
}