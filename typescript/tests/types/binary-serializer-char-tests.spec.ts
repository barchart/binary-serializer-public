import { BinarySerializerChar } from "../../src";

describe('BinarySerializerCharTests', () => {
  let serializer: BinarySerializerChar;

  beforeEach(() => {
      serializer = new BinarySerializerChar();
  });

  describe('Encode', () => {
    const testCases = [
        'a', 
        'z', 
        'A', 
        'Z', 
        '0', 
        '9', 
    '\u00A0'
    ];

    testCases.forEach((value) => {
        it(`should write expected bytes for char: ${value}`, () => {
            const writer = {
                writeBit: vi.fn(),
                writeByte: vi.fn(),
                writeBytes: vi.fn(),
                toBytes: vi.fn(),
                bookmark: vi.fn(),
                bytesWritten: 0
            };
            serializer.encode(writer, value);

            const expectedBytes = new Uint8Array(new Uint16Array([value.charCodeAt(0)]).buffer);

            expect(writer.writeBytes).toHaveBeenCalledWith(expectedBytes);
        });
    });
  });

  describe('Decode', () => {
    const testCases = [
        'a', 
        'z', 
        'A', 
        'Z', 
        '0', 
        '9', 
        '\u00A0'
    ];

    testCases.forEach((value) => {
        it(`should return expected value for encoded char: ${value}`, () => {
            const charCode = value.charCodeAt(0);
            const reader = {
                readBytes: vi.fn(),
                readBit: vi.fn(),
                readByte: vi.fn(),
                bookmark: vi.fn(),
                bytesRead: 0
            };
            reader.readBytes.mockReturnValue(new Uint8Array(new Uint16Array([charCode]).buffer));

            const deserialized = serializer.decode(reader);

            expect(deserialized).toEqual(value);
        });
    });

    it('should read from the returned byte array view offset', () => {
        const bytes = new Uint8Array([0xFF, 0x41, 0x00, 0xFF]);
        const reader = {
            readBytes: vi.fn(() => bytes.subarray(1, 3)),
            readBit: vi.fn(),
            readByte: vi.fn(),
            bookmark: vi.fn(),
            bytesRead: 0
        };

        expect(serializer.decode(reader)).toBe('A');
    });
  });

  describe('GetEquals', () => {
      const testCases = [
          ['a', 'a'],
          ['b', 'b'],
          ['A', 'A'],
          ['a', 'b'],
          ['A', 'B']
      ];

      testCases.forEach(([char1, char2]) => {
          it(`should match equals output for chars: ${char1} and ${char2}`, () => {
              const actual = serializer.getEquals(char1, char2);
              const expected = char1 === char2;

              expect(actual).toBe(expected);
          });
      });
  });
});
