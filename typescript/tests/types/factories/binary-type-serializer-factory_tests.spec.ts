import {
  BinaryTypeSerializerFactory, DataType, BinarySerializerBool, BinarySerializerChar, BinarySerializerDateOnly, BinarySerializerDateTime,
  BinarySerializerDecimal, BinarySerializerDouble, BinarySerializerEnum, BinarySerializerFloat, BinarySerializerByte, BinarySerializerShort,
  BinarySerializerInt, BinarySerializerLong, BinarySerializerSByte, BinarySerializerString, BinarySerializerUInt, BinarySerializerULong,
  BinarySerializerUShort, DataBufferWriter, DataWriter, UnsupportedTypeException
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

  describe("Enum range selection", () => {
    it("should use int storage for enums by default", () => {
      class DefaultEnum extends Enum {
        static A = new DefaultEnum("A", "A", 0);
        static B = new DefaultEnum("B", "B", 255);
       
        constructor(code: string, desc: string, mapping: number) {
          super(code, desc, mapping);
        }
      }

      const s = factory.make(DataType.enum, DefaultEnum) as BinarySerializerEnum<DefaultEnum>;
     
      expect(s).toBeInstanceOf(BinarySerializerEnum);
      expect(s.sizeInBytes).toBe(4);
    });

    it("should use byte storage when explicitly configured", () => {
      class ByteEnum extends Enum {
        static A = new ByteEnum("A", "A", 0);
        static B = new ByteEnum("B", "B", 255);
      
        constructor(code: string, desc: string, mapping: number) {
          super(code, desc, mapping);
        }
      }

      const s = factory.make(DataType.enum, ByteEnum, DataType.byte) as BinarySerializerEnum<ByteEnum>;

      expect(s.sizeInBytes).toBe(1);
    });

    it("should reject byte enum mappings outside the byte range", () => {
      class InvalidByteEnum extends Enum {
        static Value = new InvalidByteEnum("Value", "Value", 256);
      
        constructor(code: string, desc: string, mapping: number) {
          super(code, desc, mapping);
        }
      }

      expect(() => factory.make(DataType.enum, InvalidByteEnum, DataType.byte)).toThrow(RangeError);
    });

    it("should use byte storage for nullable byte enums", () => {
      class ByteEnum extends Enum {
        static Value = new ByteEnum("Value", "Value", 1);
     
        constructor(code: string, desc: string, mapping: number) {
          super(code, desc, mapping);
        }
      }
      const serializer = factory.makeNullable<ByteEnum>(DataType.enum, ByteEnum, DataType.byte);
      const writer = new DataBufferWriter(new Uint8Array(2));

      serializer.encode(writer, ByteEnum.Value);

      expect(writer.bytesWritten).toBe(2);
    });

    it("should create int enum serializer for values exceeding 2.1e9 up to int32 max", () => {
      class LargeIntEnum extends Enum {
        static A = new LargeIntEnum("A", "A", 0);
        static B = new LargeIntEnum("B", "B", 2140000000);
     
        constructor(code: string, desc: string, mapping: number) {
          super(code, desc, mapping);
        }
      }

      const s = factory.make(DataType.enum, LargeIntEnum) as BinarySerializerEnum<LargeIntEnum>;
    
      expect(s).toBeInstanceOf(BinarySerializerEnum);
      expect(s.sizeInBytes).toBe(4);
    });

    it("should not wrap the already-nullable string serializer", () => {
      const serializer = factory.makeNullable(DataType.string);

      expect(serializer).toBeInstanceOf(BinarySerializerString);
    });

    it("should write one null flag for a non-null nullable string", () => {
      const serializer = factory.makeNullable<string>(DataType.string);
      const writer = {
        writeBit: vi.fn(),
        writeByte: vi.fn(),
        writeBytes: vi.fn()
      } as unknown as DataWriter;

      serializer.encode(writer, "value");

      expect(writer.writeBit).toHaveBeenCalledTimes(1);
    });
  });
});
