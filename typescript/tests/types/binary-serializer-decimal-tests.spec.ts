import { BinarySerializerDecimal } from "../../src";
import { Helpers } from "../common/helpers";
import Big from "big.js";

describe('BinarySerializerDecimalTests', () => {
    let serializer: BinarySerializerDecimal;
    
    beforeEach(() => {
        serializer = new BinarySerializerDecimal();
    });

    describe('GetDecimalComponents', () => {
        it('should return .NET decimal components', () => {
            expect(serializer.getDecimalComponents(new Big('79228162514264337593543950335'))).toEqual([-1, -1, -1, 0]);
        });

        it('should reject precision greater than 28 decimal places', () => {
            expect(() => serializer.getDecimalComponents(new Big('1e-29'))).toThrow('Decimal precision cannot exceed 28 decimal places.');
        });

        it('should reject values exceeding the 96-bit range', () => {
            expect(() => serializer.getDecimalComponents(new Big('79228162514264337593543950336'))).toThrow('Decimal value exceeds the 96-bit range.');
        });
    });

    describe('Encode', () => {
        const testCases = [
            "0",
            "1",
            "-1",
            "79228162514264337593543950335",
            "-79228162514264337593543950335",
            "0.0000000000000000000000000001",
            "3.14159265359"
        ];

        testCases.forEach((valueString) => {
            it(`should write expected bytes for ${valueString}`, () => {
                const value = new Big(valueString);
                const writer = {
                    writeBit: vi.fn(),
                    writeByte: vi.fn(),
                    writeBytes: vi.fn(),
                    toBytes: vi.fn(),
                    bookmark: vi.fn(),
                    bytesWritten: 0
                };
                const bitsWritten: boolean[] = [];
                const byteWritten: number[] = [];
                const bytesWritten: Uint8Array[] = [];

                writer.writeBit.mockImplementation((bit: boolean) => bitsWritten.push(bit));
                writer.writeByte.mockImplementation((byte: number) => byteWritten.push(byte));
                writer.writeBytes.mockImplementation((bytes: Uint8Array) => bytesWritten.push(bytes));

                serializer.encode(writer, value);

                expect(bitsWritten.length).toBe(0);
                expect(byteWritten.length).toBe(0);

                const components = serializer.getDecimalComponents(value);
                const expectedBytes = new Uint8Array(16);

                for (let i = 0; i < 4; i++) {
                    new DataView(expectedBytes.buffer).setInt32(i * 4, components[i], true);
                }

                expect(bytesWritten.length).toBe(4);
                const bytes = Helpers.combineFourByteArrays(bytesWritten);
                expect(bytes.length).toBe(expectedBytes.length);

                for (let i = 0; i < expectedBytes.length; i++) {
                    expect(bytes[i]).toBe(expectedBytes[i]);
                }
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            "0",
            "1",
            "-1",
            "79228162514264337593543950335",
            "-79228162514264337593543950335",
            "0.0000000000000000000000000001",
            "3.14159265359"
        ];

        testCases.forEach((expectedString) => {
            it(`should return expected value for ${expectedString}`, () => {
                const expectedValue = new Big(expectedString);
                const components = serializer.getDecimalComponents(expectedValue);
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn(),
                    bytesRead: 0
                };

                reader.readBytes.mockReturnValueOnce(new Uint8Array(new Int32Array([components[0]]).buffer))
                    .mockReturnValueOnce(new Uint8Array(new Int32Array([components[1]]).buffer))
                    .mockReturnValueOnce(new Uint8Array(new Int32Array([components[2]]).buffer))
                    .mockReturnValueOnce(new Uint8Array(new Int32Array([components[3]]).buffer));

                const actualValue = serializer.decode(reader);

                expect(actualValue.eq(expectedValue)).toBe(true);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases = [
            ["0", "0"],
            ["1", "1"],
            ["-1", "-1"],
            ["79228162514264337593543950335", "79228162514264337593543950335"],
            ["-79228162514264337593543950335", "-79228162514264337593543950335"],
            ["0.0000000000000000000000000001", "0.0000000000000000000000000001"],
            ["3.14159265359", "3.14159265359"],
            ["1", "-1"],
            ["0.1", "0.2"]
        ];

        testCases.forEach(([first, second]) => {
            it(`should return expected result for ${first} and ${second}`, () => {
                const a = new Big(first);
                const b = new Big(second);
                
                const actual = serializer.getEquals(a, b);
                const expected = a.eq(b);

                expect(actual).toBe(expected);
            });
        });
    });
});
