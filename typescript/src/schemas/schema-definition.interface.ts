import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { Header } from "../headers/header";

/**
 * Serializes and deserializes instances of the TEntity class.
 *
 * @public
 * @exported
 * @template TEntity - The type of the entity that can be serialized or deserialized.
 */
export interface SchemaDefinition<TEntity extends object> {
    /**
     * The entity ID to be included in the header. This ID helps identify the type of entity the data represents.
     *
     * @public
     */
    entityId: number;

    /**
     * Serializes the source entity. In other words, this method creates a binary "snapshot" of the entity.
     *
     * @public
     * @param {DataWriter} writer - A buffer for binary data, used during the serialization process.
     * @param {TEntity} source - The entity to serialize.
     * @returns {Uint8Array} The serialized entity, as a byte array.
     * @throws {ArgumentNullException} - If the source entity is null.
     */
    serialize(writer: DataWriter, source: TEntity): Uint8Array;

    /**
     * Serializes changes between the current and previous versions of an entity.
     * In other words, this method creates a binary "delta" representing the state change between two versions of an entity.
     *
     * @public
     * @param {DataWriter} writer - A buffer for binary data, used during the serialization process.
     * @param {TEntity} current - The current version of the entity.
     * @param {TEntity} previous - The previous version of the entity.
     * @returns {Uint8Array} The serialized changes to the entity, as a byte array.
     * @throws {ArgumentNullException} - If the current or previous entity is null.
     * @throws {KeyMismatchException} - If the key schema items of the current and previous entities do not match.
     */
    serializeChanges(writer: DataWriter, current: TEntity, previous: TEntity): Uint8Array;

    /**
     * Deserializes an entity. In other words, this method recreates the serialized "snapshot" as a new instance of the TEntity class.
     *
     * @public
     * @param {DataReader} reader - A buffer of binary data which contains the serialized entity.
     * @returns {TEntity} A new instance of the TEntity class.
     * @throws {HeaderMismatchException} - If the header from the serialized byte array is invalid.
     */
    deserialize(reader: DataReader): TEntity;

    /**
     * Deserializes an entity, updating an existing instance of the TEntity class.
     *
     * @public
     * @param {DataReader} reader - A buffer of binary data which contains the serialized entity.
     * @param {TEntity} target - The target entity to assign the deserialized values to.
     * @param {boolean} [existing] - Optional flag to indicate if the target entity is an existing instance.
     * @returns {TEntity} The reference to the target instance.
     * @throws {ArgumentNullException} - If the target entity is null.
     * @throws {KeyMismatchException} - If the key schema items of the current and previous entities do not match.
     * @throws {HeaderMismatchException} - If the header from the serialized byte array is invalid.
     */
    deserializeChanges(reader: DataReader, target: TEntity, existing?: boolean): TEntity;

    /**
     * Deserializes a header from the reader.
     *
     * @public
     * @param {DataReader} reader - A buffer of binary data which contains the serialized entity.
     * @returns {Header} The header of the entity.
     */
    readHeader(reader: DataReader): Header;

    /**
     * Deserializes a key value (only) from the reader.
     *
     * @public
     * @param {DataReader} reader - A buffer of binary data which contains the serialized entity.
     * @param {string} name - The name of the key property.
     * @template TMember - The type of the key property.
     * @returns {TMember} The value of the key.
     * @throws {HeaderMismatchException} - If the header from the serialized byte array is invalid.
     * @throws {KeyUndefinedException} - If the key schema item with the given name is not found.
     */
    readKey<TMember>(reader: DataReader, name: string): TMember;

    /**
     * Performs a deep equality check of two TEntity instances.
     *
     * @public
     * @param {TEntity} a - The first entity.
     * @param {TEntity} b - The second entity.
     * @returns {boolean} True, if the serializable members of the instances are equal; otherwise false.
     */
    getEquals(a: TEntity, b: TEntity): boolean;
}