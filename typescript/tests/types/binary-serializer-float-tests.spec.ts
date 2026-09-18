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
        const expectedBytes = Number.isNaN(value) ? new Uint8Array([0x00, 0x00, 0xC0, 0xFF]) : new Uint8Array(new Float32Array([value]).buffer);

        const writer = {
          writeBit: vi.fn(),
          writeByte: vi.fn(),
          writeBytes: vi.fn(),
          toBytes: vi.fn(),
          bookmark: vi.fn(),
          bytesWritten: 0
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
          bytesRead: 0
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
      [NaN, NaN],
      [0.1, Math.fround(0.1)],
      [1, -1],
      [0.1, 0.2]
    ];

    testCases.forEach(([a, b]) => {
      it(`should match equals output for values: ${a} and ${b}`, () => {
        const actual = serializer.getEquals(a, b);

        const floatA = Math.fround(a);
        const floatB = Math.fround(b);

        const expected = floatA === floatB || (Number.isNaN(floatA) && Number.isNaN(floatB));

        expect(actual).toBe(expected);
      });
    });
  });
});
