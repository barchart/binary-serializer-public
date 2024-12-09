import {
  BinaryTypeSerializerFactory, DataType, BinarySerializerBool, BinarySerializerChar, BinarySerializerDateOnly, BinarySerializerDateTime,
  BinarySerializerDecimal, BinarySerializerDouble, BinarySerializerEnum, BinarySerializerFloat, BinarySerializerByte, BinarySerializerShort,
  BinarySerializerInt, BinarySerializerLong, BinarySerializerSByte, BinarySerializerString, BinarySerializerUInt, BinarySerializerULong,
  BinarySerializerUShort, UnsupportedTypeException
} from "../../../src";
import Enum from "@barchart/common-js/lang/Enum";

describe("BinaryTypeSerializerFactoryTests", () => {
  let factory: BinaryTypeSerializerFactory;

  beforeEach(() => {
    factory = new BinaryTypeSerializerFactory();
  });

  const supportedDataTypes = [
    { type: DataType.bool, serializer: BinarySerializerBool },
    { type: DataType.char, serializer: BinarySerializerChar },
    { type: DataType.dateonly, serializer: BinarySerializerDateOnly },
    { type: DataType.datetime, serializer: BinarySerializerDateTime },
    { type: DataType.decimal, serializer: BinarySerializerDecimal },
    { type: DataType.double, serializer: BinarySerializerDouble },
    { type: DataType.enum, serializer: BinarySerializerEnum },
    { type: DataType.float, serializer: BinarySerializerFloat },
    { type: DataType.byte, serializer: BinarySerializerByte },
    { type: DataType.short, serializer: BinarySerializerShort },
    { type: DataType.int, serializer: BinarySerializerInt },
    { type: DataType.long, serializer: BinarySerializerLong },
    { type: DataType.sbyte, serializer: BinarySerializerSByte },
    { type: DataType.string, serializer: BinarySerializerString },
    { type: DataType.ushort, serializer: BinarySerializerUShort },
    { type: DataType.uint, serializer: BinarySerializerUInt },
    { type: DataType.ulong, serializer: BinarySerializerULong },
  ];

  const unsupportedDataTypes = [
    DataType.list,
    DataType.object
  ];

  supportedDataTypes.forEach(({ type, serializer }) => {
    it(`should support the ${DataType[type]} data type`, () => {
      const isSupported = factory.supports(type);
      expect(isSupported).toBe(true);
    });

    it(`should create a serializer for the ${DataType[type]} data type`, () => {
      const createdSerializer = factory.make(type, type === DataType.enum ? Enum : undefined);

      expect(createdSerializer).toBeDefined();
      expect(createdSerializer).toBeInstanceOf(serializer);
    });
  });

  unsupportedDataTypes.forEach((type) => {
    it(`should not support the ${DataType[type]} data type`, () => {
        const isSupported = factory.supports(type);
        expect(isSupported).toBe(false);
    });

    it(`should throw an UnsupportedTypeException when creating a serializer for the unsupported ${DataType[type]} data type`, () => {
        expect(() => factory.make(type)).toThrowError(UnsupportedTypeException);
    });
  });
  
});