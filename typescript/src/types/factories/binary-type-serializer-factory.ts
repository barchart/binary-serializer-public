import { BinarySerializerBool } from "../binary-serializer-bool";
import { BinarySerializerByte } from "../binary-serializer-byte";
import { BinarySerializerChar } from "../binary-serializer-char";
import { BinarySerializerDateOnly } from "../binary-serializer-dateonly";
import { BinarySerializerDateTime } from "../binary-serializer-datetime";
import { BinarySerializerDecimal } from "../binary-serializer-decimal";
import { BinarySerializerDouble } from "../binary-serializer-double";
import { BinarySerializerEnum } from "../binary-serializer-enum";
import { BinarySerializerFloat } from "../binary-serializer-float";
import { BinarySerializerInt } from "../binary-serializer-int";
import { BinarySerializerLong } from "../binary-serializer-long";
import { BinarySerializerSByte } from "../binary-serializer-sbyte";
import { BinarySerializerShort } from "../binary-serializer-short";
import { BinarySerializerString } from "../binary-serializer-string";
import { BinarySerializerUInt } from "../binary-serializer-uint";
import { BinarySerializerULong } from "../binary-serializer-ulong";
import { BinarySerializerUShort } from "../binary-serializer-ushort";
import { BinaryTypeSerializer, BinaryTypeSerializerBase } from "../binary-type-serializer.interface";
import { DataType } from "../data-types";
import { UnsupportedTypeException } from "../exceptions/unsupported-type-exception";
import { SerializerFactory } from "./serializer-factory.interface";
import { BinarySerializerNullable } from "../binary-serializer-nullable";
import Enum from "@barchart/common-js/lang/Enum";

const BYTE_MINIMUM_VALUE = 0;
const BYTE_MAXIMUM_VALUE = 0xFF;
const INT_MINIMUM_VALUE = -0x80000000;
const INT_MAXIMUM_VALUE = 0x7FFFFFFF;

/**
 * Defines a factory for creating binary type serializers.
 *
 * @public
 * @exported
 * @implements {SerializerFactory}
 */
export class BinaryTypeSerializerFactory implements SerializerFactory {
    private readonly serializers: Map<DataType, BinaryTypeSerializerBase> = new Map();

    initializeSerializers() {
        this.serializers.set(DataType.bool, new BinarySerializerBool());
        this.serializers.set(DataType.byte, new BinarySerializerByte());
        this.serializers.set(DataType.char, new BinarySerializerChar());
        this.serializers.set(DataType.dateonly, new BinarySerializerDateOnly());
        this.serializers.set(DataType.datetime, new BinarySerializerDateTime());
        this.serializers.set(DataType.decimal, new BinarySerializerDecimal());
        this.serializers.set(DataType.double, new BinarySerializerDouble());
        this.serializers.set(DataType.float, new BinarySerializerFloat());
        this.serializers.set(DataType.int, new BinarySerializerInt());
        this.serializers.set(DataType.long, new BinarySerializerLong());
        this.serializers.set(DataType.sbyte, new BinarySerializerSByte());
        this.serializers.set(DataType.short, new BinarySerializerShort());
        this.serializers.set(DataType.uint, new BinarySerializerUInt());
        this.serializers.set(DataType.ulong, new BinarySerializerULong());
        this.serializers.set(DataType.ushort, new BinarySerializerUShort());
        this.serializers.set(DataType.string, new BinarySerializerString());
    }

    constructor() {
        this.initializeSerializers();
    }

    supports(dataType: DataType): boolean {
        return this.serializers.has(dataType) || dataType === DataType.enum;
    }

    make<T>(dataType: DataType, enumType?: new (...args: any[]) => Enum, enumUnderlyingType: DataType.byte | DataType.int = DataType.int): BinaryTypeSerializer<T> {
        const serializer = this.serializers.get(dataType);

        if (serializer){
            return serializer as BinaryTypeSerializer<T>;
        }

        if (dataType === DataType.enum) {
            if (!enumType) {
                throw new Error("Enum type is required for DataType.enum");
            }

            return this.createEnumSerializer(enumType, enumUnderlyingType) as BinaryTypeSerializer<T>;
        }

        throw new UnsupportedTypeException(dataType);
    }

    makeNullable<T>(dataType: DataType, enumType?: new (...args: any[]) => Enum, enumUnderlyingType: DataType.byte | DataType.int = DataType.int): BinaryTypeSerializer<T | null> {
        const serializer = this.make<T>(dataType, enumType, enumUnderlyingType);

        if (dataType === DataType.string) {
            return serializer as BinaryTypeSerializer<T | null>;
        }

        return new BinarySerializerNullable<T>(serializer);
    }

    private createEnumSerializer(enumType: new (...args: any[]) => Enum, storageType: DataType.byte | DataType.int): BinarySerializerEnum<any> {
        const enumItems: Enum[] = Enum.getItems(enumType);
        const mappings: number[] = enumItems.map(item => item.mapping).filter(mapping => mapping !== null) as number[];

        const smallest = Math.min(...mappings);
        const largest = Math.max(...mappings);

        if (storageType === DataType.byte && smallest >= BYTE_MINIMUM_VALUE && largest <= BYTE_MAXIMUM_VALUE) {
            return new BinarySerializerEnum(this.serializers.get(DataType.byte) as BinarySerializerByte, enumType);
        } else if (storageType === DataType.int && smallest >= INT_MINIMUM_VALUE && largest <= INT_MAXIMUM_VALUE) {
            return new BinarySerializerEnum(this.serializers.get(DataType.int) as BinarySerializerInt, enumType);
        } else {
            throw new RangeError("Unsupported enum mapping range");
        }
    }
}
