import { DataType } from "../types/data-types";
import Enum from "@barchart/common-js/lang/Enum";

/**
 * Represents a field in a data structure that is a simple type, including support for nullable types.
 *
 * @public
 * @exported
 */
export type SchemaPrimitiveField = {
    /**
     * The name of the field.
     */
    name: string;

    /**
     * The data type of the field. It can be a primitive or nullable primitive type.
     */
    type: Exclude<DataType, DataType.list | DataType.object>;

    /**
     * Indicates whether the field is a key.
     */
    isKey?: boolean;

    /**
     * Indicates whether the field is nullable.
     */
    nullable?: boolean;

    /**
     * The enumeration type if the field is an enumeration.
     */
    enumType?: new (...args: any[]) => Enum;
}

/**
 * Represents a field in a data structure that is a list.
 *
 * @public
 * @exported
 */
export type SchemaListField =
    | {
    /**
     * The name of the field.
     */
    name: string;

    /**
     * The data type of the field.
     */
    type: DataType.list;

    /**
     * The data type of the elements if the field is a list or array.
     * This property is optional.
     */
    elementType: Exclude<DataType, DataType.object>;
}
    | {
    /**
     * The name of the field.
     */
    name: string;

    /**
     * The data type of the field.
     */
    type: DataType.list;

    /**
     * The data type of the elements if the field is a list or array.
     */
    elementType: DataType.object;

    /**
     * Nested fields if the field is an object containing other fields.
     */
    fields: SchemaField[];
};

/**
 * Represents a field in a data structure that is an object.
 *
 * @public
 * @exported
 */
export type SchemaNestedObjectField = {
    /**
     * The name of the field.
     */
    name: string;

    /**
     * The data type of the field.
     */
    type: DataType.object;

    /**
     * Nested fields if the field is an object containing other fields.
     */
    fields: SchemaField[];
};

/**
 * Represents a field in a data structure.
 *
 * @public
 * @exported
 */
export type SchemaField = SchemaPrimitiveField | SchemaNestedObjectField | SchemaListField;