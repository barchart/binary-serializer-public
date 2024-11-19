import { BinarySerializerDouble } from "../../src";

describe('BinarySerializerDoubleTests', () => {
  let serializer: BinarySerializerDouble;

  beforeEach(() => {
    serializer = new BinarySerializerDouble();
  });

  describe('Encode', () => {
    const testCases: number[] = [
      0, 
      1, 
      -1, 
      Number.MAX_VALUE, 
      Number.MIN_VALUE, 
      Number.EPSILON, 
      Math.PI, 
      Infinity, 
      -Infinity,
      NaN
    ];

    testCases.forEach(value => {
      it(`should write expected bytes for value: ${value}`, () => {
        const writer = {
          writeBit: vi.fn(),
          writeByte: vi.fn(),
          writeBytes: vi.fn(),
          toBytes: vi.fn(),
          bookmark: vi.fn(),
          bytesWritten: 0
        };

        serializer.encode(writer, value);

        const expectedBytes = new Uint8Array(new Float64Array([value]).buffer);

        expect(writer.writeBytes).toHaveBeenCalledWith(expectedBytes);
      });
    });
  });

  describe('Decode', () => {
    const testCases: number[] = [
      0, 
      1, 
      -1, 
      Number.MAX_VALUE, 
      Number.MIN_VALUE, 
      Number.EPSILON, 
      Math.PI, 
      Infinity, 
      -Infinity, 
      NaN
    ];

    testCases.forEach(value => {
      it(`should return expected value for encoded bytes: ${value}`, () => {
        const bytes = new Uint8Array(new Float64Array([value]).buffer);
        const reader = {
          readBytes: vi.fn(),
          readBit: vi.fn(),
          readByte: vi.fn(),
          bookmark: vi.fn(),
          bytesRead: 0
        };
        reader.readBytes.mockReturnValue(bytes);

        const decoded = serializer.decode(reader);

        if (isNaN(value)) {
          expect(decoded).toBeNaN();
        } 
        else {
          expect(decoded).toEqual(value);
        }
      });
    });
  });

  describe('GetEquals', () => {
    const testCases: [number, number][] = [
      [0, 0],
      [1, 1],
      [-1, -1],
      [Number.MAX_VALUE, Number.MAX_VALUE],
      [Number.MIN_VALUE, Number.MIN_VALUE],
      [Number.EPSILON, Number.EPSILON],
      [Math.PI, Math.PI],
      [1, -1],
      [0.1, 0.2]
    ];

    testCases.forEach(([a, b]) => {
      it(`should match equals output for values: ${a} and ${b}`, () => {
        const actual = serializer.getEquals(a, b);
        const expected = Object.is(a, b);

        expect(actual).toBe(expected);
      });
    });
  });
});