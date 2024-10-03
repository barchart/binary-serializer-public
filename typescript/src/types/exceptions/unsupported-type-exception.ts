import { DataType } from "../data-types";

/**
 * Thrown when an BinaryTypeSerializerFactory is unable to
 * create an BinaryDataSerializerFactory for the requested type.
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {DataType} unsupported - The unsupported type.
 */
export class UnsupportedTypeException extends Error {
    constructor(unsupported: DataType) {
        super(`Unable to create a serializer for the (${unsupported}) type.`);

        Object.setPrototypeOf(this, UnsupportedTypeException.prototype);
    }
}