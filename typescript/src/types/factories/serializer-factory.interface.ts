import { BinaryTypeSerializer } from "../binary-type-serializer.interface";
import { DataType } from "../data-types";
import Enum from "@barchart/common-js/lang/Enum";

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
     * @param {DataType} dataType - The type to create a serializer for.
     * @param {new (...args: any[]) => Enum} [enumType] - Optional, the enumeration type (if the specified type is an enumeration).
     * @returns An BinaryTypeSerializer for the specified type.
     * @throws {UnsupportedTypeException} Thrown when the factory is unable to create a serializer for the specified type.
     */
    make<T>(dataType: DataType, enumType?: new (...args: any[]) => Enum): BinaryTypeSerializer<T>;

    /**
     * Creates a binary type serializer for the specified nullable type.
     *
     * @public
     * @param {DataType} dataType - The type to create a serializer for.
     * @param {new (...args: any[]) => Enum} [enumType] - Optional, the enumeration type (if the specified type is an enumeration).
     * @returns An BinaryTypeSerializer for the specified type.
     * @throws {UnsupportedTypeException} Thrown when the factory is unable to create a serializer for the specified type.
     */
    makeNullable<T>(dataType: DataType, enumType?: new (...args: any[]) => Enum): BinaryTypeSerializer<T | null>;
}