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
        this.serializers.set(DataType.enum, new BinarySerializerEnum(this.serializers.get(DataType.int) as BinarySerializerInt));
    }

    constructor() {
        this.initializeSerializers();
    }

    supports(dataType: DataType): boolean {
        return this.serializers.has(dataType);
    }

    make<T>(dataType: DataType): BinaryTypeSerializer<T> {
        const serializer = this.serializers.get(dataType);

        if (!serializer) {
            throw new UnsupportedTypeException(dataType);
        }

        return serializer as BinaryTypeSerializer<T>;
    }

    makeNullable<T>(dataType: DataType): BinaryTypeSerializer<T | null> {
        const serializer = this.make<T>(dataType);

        return new BinarySerializerNullable<T>(serializer);
    }
}