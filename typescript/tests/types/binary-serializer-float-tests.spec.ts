import { BinarySerializerFloat } from "../../src";

describe('BinarySerializerFloatTests', () => {
  let serializer: BinarySerializerFloat;

  beforeEach(() => {
    serializer = new BinarySerializerFloat();
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
        const expectedBytes = new Uint8Array(new Float32Array([value]).buffer);
        const writer = {
          writeBit: vi.fn(),
          writeByte: vi.fn(),
          writeBytes: vi.fn(),
          toBytes: vi.fn(),
          bytesWritten: vi.fn(),
          bookmark: vi.fn(),
          isAtRootNestingLevel: true
        };

        serializer.encode(writer, value);

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
      it(`should return expected float value for encoded bytes: ${value}`, () => {
        const bytes = new Uint8Array(new Float32Array([value]).buffer);
        const reader = {
          readBytes: vi.fn(),
          readBit: vi.fn(),
          readByte: vi.fn(),
          bookmark: vi.fn(),
          isAtRootNestingLevel: true
        };
        reader.readBytes.mockReturnValue(bytes);

        const decoded = serializer.decode(reader);

        if (isNaN(value)) {
          expect(decoded).toBeNaN();
        }
        else if (value === Infinity || value === -Infinity) {
          expect(decoded).toBe(value);
        }
        else if (value === Number.MAX_VALUE) {
          expect(decoded).toBe(Infinity);
        }
        else {
          expect(decoded).toBeCloseTo(value, 6);
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