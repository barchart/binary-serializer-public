import { SchemaFactory } from "../schemas/factories/schema-factory";
import { DataBufferReaderFactory } from "../buffers/factories/data-buffer-reader-factory";
import { DataBufferWriterFactory } from "../buffers/factories/data-buffer-writer-factory";
import { Serializer } from "./serializer";
import { SchemaDefinition } from "../schemas/schema-definition.interface";
import { SchemaField } from "../schemas/schema-field.type";
import { SerializerFactory } from "../types/factories/serializer-factory.interface";
import { DataWriterFactory } from "../buffers/factories/data-writer-factory.interface";
import { DataReaderFactory } from "../buffers/factories/data-reader-factory.interface";
import { SerializationSchemaFactory } from "../schemas/factories/serialization-schema-factory.interface";

/**
 * A builder class for constructing instances of `Serializer<TEntity>`.
 *
 * @public
 * @exported
 * @template TEntity - The type of entity to be (de)serialized.
 * @param {number} entityId - The unique identifier for the entity.
 */
export class SerializerBuilder<TEntity extends object> {
    private schemaFactory: SerializationSchemaFactory;

    private dataReaderFactory: DataReaderFactory;
    private dataWriterFactory: DataWriterFactory;

    private readonly entityId: number;

    constructor(entityId: number = 0) {
        this.schemaFactory = new SchemaFactory();

        this.dataReaderFactory = new DataBufferReaderFactory();
        this.dataWriterFactory = new DataBufferWriterFactory();

        this.entityId = entityId;
    }

    /**
     * Specifies the schema factory to be used by the serializer.
     *
     * @public
     * @param {SerializationSchemaFactory} schemaFactory - The schema factory to be used.
     * @returns {SerializerBuilder<TEntity>} The current instance of `SerializerBuilder<TEntity>` for method chaining.
     */
    withSchemaFactory(schemaFactory: SerializationSchemaFactory): SerializerBuilder<TEntity> {
        this.schemaFactory = schemaFactory;
        return this;
    }

    /**
     * Specifies the schema factory using a type serializer factory.
     *
     * @public
     * @param {SerializerFactory} typeFactory - The type serializer factory to be used for creating the schema factory.
     * @returns {SerializerBuilder<TEntity>} The current instance of `SerializerBuilder<TEntity>` for method chaining.
     */
    withSchemaFactoryUsingBinaryTypeSerializerFactory(typeFactory: SerializerFactory): SerializerBuilder<TEntity> {
        this.schemaFactory = new SchemaFactory(typeFactory);
        return this;
    }

    /**
     * Specifies the data buffer reader factory to be used by the serializer.
     *
     * @public
     * @param {DataReaderFactory} dataReaderFactory - The data buffer reader factory to be used.
     * @returns {SerializerBuilder<TEntity>} The current instance of `SerializerBuilder<TEntity>` for method chaining.
     */
    withDataBufferReaderFactory(dataReaderFactory: DataReaderFactory): SerializerBuilder<TEntity> {
        this.dataReaderFactory = dataReaderFactory;
        return this;
    }

    /**
     * Specifies the data buffer writer factory to be used by the serializer.
     *
     * @public
     * @param {DataWriterFactory} dataWriterFactory - The data buffer writer factory to be used.
     * @returns {SerializerBuilder<TEntity>} The current instance of `SerializerBuilder<TEntity>` for method chaining.
     */
    withDataBufferWriterFactory(dataWriterFactory: DataWriterFactory): SerializerBuilder<TEntity> {
        this.dataWriterFactory = dataWriterFactory;
        return this;
    }

    /**
     * Builds and returns a configured instance of `Serializer<TEntity>`.
     *
     * @public
     * @param {SchemaField[]} fields - The schema fields to be used by the serializer.
     * @returns {Serializer<TEntity>} An instance of `Serializer<TEntity>` configured with the specified factories.
     */
    build(fields: SchemaField[]): Serializer<TEntity> {
        const schema: SchemaDefinition<TEntity> = this.schemaFactory.make(this.entityId, fields);
        return new Serializer<TEntity>(this.entityId, fields, schema, this.dataReaderFactory, this.dataWriterFactory);
    }

    /**
     * Creates a new instance of `SerializerBuilder<TEntity>` for the specified entity type.
     *
     * @public
     * @static
     * @returns {SerializerBuilder<TEntity>} A new instance of `SerializerBuilder<TEntity>`.
     */
    static forType<TEntity extends object>(entityId: number = 0): SerializerBuilder<TEntity> {
        return new SerializerBuilder<TEntity>(entityId);
    }
}