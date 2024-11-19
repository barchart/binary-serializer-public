import { BinarySerializerString } from "../../src";

describe('BinarySerializerStringTests', () => {
    let serializer: BinarySerializerString;

    beforeEach(() => {
        serializer = new BinarySerializerString();
    });

    describe('Encode', () => {
        const testCases: string[] = [
            "Testing Encoding & Decoding methods",
            "",
            "Binary Serialization"
        ];

        testCases.forEach((value) => {
            it(`should write expected bytes for "${value}"`, () => {
               const writer = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bookmark: vi.fn(),
                    bytesWritten: 0
               };
               const bytesWritten: Uint8Array[] = [];

                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesWritten.push(bytes));

                serializer.encode(writer, value);

                expect(bytesWritten.length).toBe(2);

                const expectedLengthBytes = new Uint8Array(new Int16Array([value.length]).buffer);
                expect(Array.from(bytesWritten[0])).toEqual(Array.from(expectedLengthBytes));

                const expectedContentBytes = new TextEncoder().encode(value);
                expect(Array.from(bytesWritten[1])).toEqual(Array.from(expectedContentBytes));
            });
        });
    });

    describe('Decode', () => {
        const testCases: string[] = [
            "Testing Encoding & Decoding methods",
            "",
            "Binary Serialization"
        ];

        testCases.forEach((expectedValue) => {
            it(`should return expected value for "${expectedValue}"`, () => {
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn(),
                    bytesRead: 0
                };
                const encodedLengthBytes = new Uint8Array(new Int16Array([expectedValue.length]).buffer);
                const encodedContentBytes = new TextEncoder().encode(expectedValue);

                reader.readBytes.mockImplementation((length: number) => {
                    if (length === 2) {
                        return encodedLengthBytes;
                    } 
                    else {
                        return encodedContentBytes;
                    }
                });

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(expectedValue);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [string, string][] = [
            ["Test", "Test"],
            ["String", "string"],
            ["", ""],
            ["Binary", "Serialization"]
        ];

        testCases.forEach(([first, second]) => {
            it(`should return expected result for "${first}" and "${second}"`, () => {
                const actual = serializer.getEquals(first, second);
                const expected = first === second;

                expect(actual).toBe(expected);
            });
        });
    });
});