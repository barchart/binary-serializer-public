import { DataReaderFactory } from "../buffers/factories/data-reader-factory.interface";
import { DataWriterFactory } from "../buffers/factories/data-writer-factory.interface";
import { DataBufferReaderFactory } from "../buffers/factories/data-buffer-reader-factory";
import { SchemaFactory } from "../schemas/factories/schema-factory";
import { SchemaDefinition } from "../schemas/schema-definition.interface";
import { SchemaField } from "../schemas/schema-field.type";
import { DataBufferWriterFactory } from "../buffers/factories/data-buffer-writer-factory";
import { Header } from "../headers/header";

/**
 * Provides (de)serialization functionality for entities of type `TEntity`.
 *
 * @public
 * @exported
 * @template TEntity - The type of entity to be (de)serialized.
 * @param {number} entityId - The unique identifier for the entity.
 * @param {SchemaField[]} fields - The fields of the entity to be (de)serialized.
 * @param {SchemaDefinition<TEntity>} [schema] - The schema definition for the entity (optional).
 * @param {DataReaderFactory} dataBufferReaderFactory - The factory for creating data buffer readers (optional).
 * @param {DataWriterFactory} dataBufferWriterFactory - The factory for creating data buffer writers (optional).
 */
export class Serializer<TEntity extends object> {
    private readonly schema: SchemaDefinition<TEntity>;
    private readonly dataBufferReaderFactory: DataReaderFactory;
    private readonly dataBufferWriterFactory: DataWriterFactory;

    constructor(entityId: number, fields: SchemaField[]);
    constructor(entityId: number, fields: SchemaField[], schema?: SchemaDefinition<TEntity>, dataBufferReaderFactory?: DataReaderFactory, dataBufferWriterFactory?: DataWriterFactory);
    constructor(entityId: number = 0, fields: SchemaField[], schema?: SchemaDefinition<TEntity>, dataBufferReaderFactory?: DataReaderFactory, dataBufferWriterFactory?: DataWriterFactory) {
        if (schema && dataBufferReaderFactory && dataBufferWriterFactory) {
            this.schema = schema;
            this.dataBufferReaderFactory = dataBufferReaderFactory;
            this.dataBufferWriterFactory = dataBufferWriterFactory;
        } else {
            const schemaFactory = new SchemaFactory();
            this.schema = schemaFactory.make<TEntity>(entityId, fields);
            this.dataBufferReaderFactory = new DataBufferReaderFactory();
            this.dataBufferWriterFactory = new DataBufferWriterFactory();
        }
    }

    /**
     * Serializes the `source` entity. In other words,
     * this method creates a binary "snapshot" of the entity.
     *
     * @public
     * @param {TEntity} source - The entity to serialize.
     * @returns {Uint8Array} The serialized entity, as a byte array.
     */
    serialize(source: TEntity): Uint8Array {
        const writer = this.dataBufferWriterFactory.make();
        return this.schema.serialize(writer, source);
    }

    /**
     * Serializes changes between the `current` and
     * `previous` versions of an entity. In other words,
     * this method creates a binary "delta" representing the state change
     * between two versions of an entity.
     *
     * @public
     * @param {TEntity} current - The current version of the entity.
     * @param {TEntity} previous - The previous version of the entity.
     * @returns {Uint8Array} The serialized changes to the entity, as a byte array.
     */
    serializeChanges(current: TEntity, previous: TEntity): Uint8Array {
        const writer = this.dataBufferWriterFactory.make();
        return this.schema.serializeChanges(writer, current, previous);
    }

    /**
     * Deserializes an entity. In other words, this method recreates the serialized
     * "snapshot" as a new instance of the `TEntity` class.
     *
     * @public
     * @param {Uint8Array} serialized - The byte array that contains serialized data.
     * @returns {TEntity} A new instance of the `TEntity` class.
     */
    deserialize(serialized: Uint8Array): TEntity {
        const reader = this.dataBufferReaderFactory.make(serialized);
        return this.schema.deserialize(reader);
    }

    /**
     * Deserializes an entity, updating an existing instance of
     * the `TEntity` class.
     *
     * @public
     * @param {Uint8Array} serialized - The byte array that contains serialized data.
     * @param {TEntity} target - The target entity to populate with deserialized data.
     * @returns {TEntity} The reference to the `target` instance.
     */
    deserializeChanges(serialized: Uint8Array, target: TEntity): TEntity {
        const reader = this.dataBufferReaderFactory.make(serialized);
        return this.schema.deserializeChanges(reader, target);
    }

    /**
     * Deserializes a header from the `serialized`.
     *
     * @public
     * @param {Uint8Array} serialized - A byte array of binary data which contains the serialized entity.
     * @returns {Header} The header of the entity.
     */
    readHeader(serialized: Uint8Array): Header {
        const reader = this.dataBufferReaderFactory.make(serialized);
        return this.schema.readHeader(reader);
    }

    /**
     * Deserializes a key value (only) from the `serialized`.
     *
     * @public
     * @template TMember - The type of the key value.
     * @param {Uint8Array} serialized - A byte array of binary data which contains the serialized entity.
     * @param {string} name - The name of the key property.
     * @returns {TMember} The value of the key.
     */
    readKey<TMember>(serialized: Uint8Array, name: string): TMember {
        const reader = this.dataBufferReaderFactory.make(serialized);
        return this.schema.readKey<TMember>(reader, name);
    }

    /**
     * Sets the non-nullable properties from the `source` entity to the `target` entity.
     *
     * @public
     * @template TEntity - The type of the entity.
     * @param {TEntity} target - The entity to update.
     * @param {TEntity} source - The entity to update from.
     * @throws {Error} If the keys of the `source` and `target` entities are not equal.
     */
    applyChanges(target: TEntity, source: TEntity): void {
        this.schema.applyChanges(target, source);
    }

    /**
     * Performs a deep equality check of two `TEntity` instances.
     *
     * @public
     * @param {TEntity} a - The first entity.
     * @param {TEntity} b - The second entity.
     * @returns {boolean} True, if the serializable members of the instances are equal; otherwise false.
     */
    getEquals(a: TEntity, b: TEntity): boolean {
        return this.schema.getEquals(a, b);
    }
}