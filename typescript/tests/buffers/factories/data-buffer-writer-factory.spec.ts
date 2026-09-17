import { DataBufferWriterFactory, DataBufferWriter, DataWriter } from '../../../src';

describe('DataBufferWriterFactoryTests', () => {
  let factory: DataBufferWriterFactory;

  beforeEach(() => {
    factory = new DataBufferWriterFactory();
  });

  describe('Constructor', () => {
    it('should throw an exception when instantiated with zero byte array size', () => {
      expect(() => new DataBufferWriterFactory(0)).toThrow(RangeError);
    });

    it('should throw an exception when instantiated with negative byte array size', () => {
      expect(() => new DataBufferWriterFactory(-1)).toThrow(RangeError);
    });

    it.each([1.5, NaN, Infinity])('should throw an exception when instantiated with invalid byte array size', size => {
      expect(() => new DataBufferWriterFactory(size)).toThrow(RangeError);
    });
  });

  describe('Make', () => {
    it('should return an DataBufferWriter instance with the default factory', () => {
      const writer: DataWriter = factory.make();

      expect(writer).toBeInstanceOf(DataBufferWriter);
    });
  });
});
