import Enum from "@barchart/common-js/lang/Enum";

class TestEnum extends Enum {
    static Value1 = new TestEnum('Value1', 'Description for Value1', 1);
    static Value2 = new TestEnum('Value2', 'Description for Value2', 2);
    static Value3 = new TestEnum('Value3', 'Description for Value3', 3);

    constructor(code: string, description: string, mapping: number) {
        super(code, description, mapping);
    }
}

import { BinarySerializerEnum, BinarySerializerInt } from "../../src";

describe('BinarySerializerEnumTests', () => {
    let serializer: BinarySerializerEnum<TestEnum>;

    beforeEach(() => {
        serializer = new BinarySerializerEnum<TestEnum>(new BinarySerializerInt(), TestEnum);
    });

    describe('Encode', () => {
        const testCases = [
            TestEnum.Value1,
            TestEnum.Value2,
            TestEnum.Value3
        ];

        testCases.forEach((value) => {
            it(`should write expected bytes for ${value.code}`, () => {
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

                const intValue = value.mapping as number;
                const expectedBytes = new Uint8Array(new Int32Array([intValue]).buffer);

                expect(bytesWritten.length).toBe(1);
                expect(bytesWritten[0].length).toBe(expectedBytes.length);

                for (let i = 0; i < expectedBytes.length; i++) {
                    expect(bytesWritten[0][i]).toBe(expectedBytes[i]);
                }
            });
        });
    });

    describe('Decode', () => {
        const testCases = [
            TestEnum.Value1,
            TestEnum.Value2,
            TestEnum.Value3
        ];

        testCases.forEach((expectedValue) => {
            it(`should return expected value for ${expectedValue.code}`, () => {
                const intValue = expectedValue.mapping as number;
                const reader = {
                    readBytes: vi.fn(),
                    readBit: vi.fn(),
                    readByte: vi.fn(),
                    bookmark: vi.fn(),
                    bytesRead: 0
                };
                reader.readBytes.mockReturnValue(new Uint8Array(new Int32Array([intValue]).buffer));

                const actualValue = serializer.decode(reader);

                expect(actualValue).toBe(expectedValue);
            });
        });
    });

    describe('GetEquals', () => {
        const testCases: [TestEnum, TestEnum][] = [
            [TestEnum.Value1, TestEnum.Value1],
            [TestEnum.Value2, TestEnum.Value2],
            [TestEnum.Value3, TestEnum.Value3],
            [TestEnum.Value1, TestEnum.Value2],
            [TestEnum.Value2, TestEnum.Value3],
            [TestEnum.Value1, TestEnum.Value3]
        ];

        testCases.forEach(([first, second]) => {
            it(`should return expected result for ${first.code} and ${second.code}`, () => {
                const actual = serializer.getEquals(first, second);
                const expected = first === second;

                expect(actual).toBe(expected);
            });
        });
    });
});