import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";

/**
 * Interface for member data definitions used in schema-based (de)serialization.
 *
 * @public
 * @exported
 * @template TEntity - The type of the container class that holds the member.
 */
export interface SchemaItemDefinition<TEntity> {
    /**
    *  Indicates if the data point is the primary key of the source object (or a member of a composite key).
    *
    *  @public
    */
    key: boolean;

    /**
     * The name of the member.
     *
     * @public
     */
    name: string;
    
    /**
     * Encodes the member value into the provided binary data writer.
     *
     * @public
     * @param {DataWriter} writer - The DataWriter to write the encoded data to.
     * @param {TEntity} source - The container object whose member value is to be encoded.
     */
    encode(writer: DataWriter, source: TEntity): void;

    /**
     * Encodes the differences between the new and old member values into the provided binary data writer.
     *
     * @public
     * @param {DataWriter} writer - The DataWriter to write the encoded data to.
     * @param {TEntity} current - The current object to read data from.
     * @param {TEntity} previous - The previous object to read data from.
     */
    encodeChanges(writer: DataWriter, current: TEntity, previous: TEntity): void;

    /**
     * Decodes the member value from the provided binary data reader and assigns it to the container.
     *
     * @public
     * @param {DataReader} reader - The DataReader containing the serialized member data.
     * @param {TEntity} target - The container instance where the decoded value will be assigned.
     * @param {boolean} [existing] - Indicates whether the target object already exists (optional).
     */
    decode(reader: DataReader, target: TEntity, existing?: boolean): void;


    /**
     * Sets the non-nullable properties from the source entity to the target entity.
     *
     * @public
     * @param {TEntity} target - The entity to update.
     * @param {TEntity} source - The entity to update from.
     */
    applyChanges(target: TEntity, source: TEntity): void;

    /**
     * Indicates whether two data points, read from the entities, are equal.
     *
     * @public
     * @param {TEntity} a The first entity (containing the first data point).
     * @param {TEntity} b The second entity (containing the second data point).
     * @returns {boolean} True if the data points are equal, false otherwise.
     */
    getEquals(a: TEntity, b: TEntity): boolean;
}

/**
 * Defines a generic schema item capable of reading a property value from a source entity.
 *
 * @public
 * @exported
 * @extends {SchemaItemDefinition<TEntity>}
 * @template TEntity - The type of the entity from which the property value is read.
 * @template TMember - The type of the property value to be read from the entity.
 */
export interface SchemaItemWithKeyDefinition<TEntity, TMember> extends SchemaItemDefinition<TEntity> {
    /**
     * Reads and returns the property value from the specified source entity.
     *
     * @public
     * @param {TEntity} source - The source entity from which to read the property value.
     * @returns {TMember} The value of the property read from the source entity.
     */
    read(source: TEntity): TMember;
}