import { BinaryTypeSerializer } from "../binary-type-serializer.interface";
import { DataType } from "../data-types";

/**
 * Defines a factory for creating binary type serializers.
 *
 * @public
 * @exported
 */
export interface SerializerFactory {
    /**
     * Indicates if the factory can make an binaryTypeSerializer for the specified type.
     *
     * @public
     * @param {DataType} dataType - The type to test.
     * @returns {boolean} True, if the factory can make an binaryTypeSerializer for the specified type.
     */
    supports(dataType: DataType): boolean;

    /**
     * Creates a binary type serializer for the specified type.
     *
     * @public
     * @returns An BinaryTypeSerializer for the specified type.
     * @throws {UnsupportedTypeException} Thrown when the factory is unable to create a serializer for the specified type.
     */
    make<T>(dataType: DataType): BinaryTypeSerializer<T>;
}