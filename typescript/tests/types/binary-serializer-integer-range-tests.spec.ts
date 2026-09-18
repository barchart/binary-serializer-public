import {
    BinarySerializerByte,
    BinarySerializerInt,
    BinarySerializerLong,
    BinarySerializerSByte,
    BinarySerializerShort,
    BinarySerializerUInt,
    BinarySerializerULong,
    BinarySerializerUShort,
    DataBufferWriter
} from '../../src';

describe('Integer serializer range validation', () => {
    const makeWriter = () => new DataBufferWriter(new Uint8Array(8));

    const cases: Array<[string, () => void]> = [
        ['byte below minimum', () => new BinarySerializerByte().encode(makeWriter(), -1)],
        ['byte above maximum', () => new BinarySerializerByte().encode(makeWriter(), 256)],
        ['sbyte below minimum', () => new BinarySerializerSByte().encode(makeWriter(), -129)],
        ['sbyte above maximum', () => new BinarySerializerSByte().encode(makeWriter(), 128)],
        ['short below minimum', () => new BinarySerializerShort().encode(makeWriter(), -32769)],
        ['short above maximum', () => new BinarySerializerShort().encode(makeWriter(), 32768)],
        ['ushort below minimum', () => new BinarySerializerUShort().encode(makeWriter(), -1)],
        ['ushort above maximum', () => new BinarySerializerUShort().encode(makeWriter(), 65536)],
        ['int below minimum', () => new BinarySerializerInt().encode(makeWriter(), -2147483649)],
        ['int above maximum', () => new BinarySerializerInt().encode(makeWriter(), 2147483648)],
        ['uint below minimum', () => new BinarySerializerUInt().encode(makeWriter(), -1)],
        ['uint above maximum', () => new BinarySerializerUInt().encode(makeWriter(), 4294967296)],
        ['long below minimum', () => new BinarySerializerLong().encode(makeWriter(), BigInt('-9223372036854775809'))],
        ['long above maximum', () => new BinarySerializerLong().encode(makeWriter(), BigInt('9223372036854775808'))],
        ['ulong below minimum', () => new BinarySerializerULong().encode(makeWriter(), BigInt(-1))],
        ['ulong above maximum', () => new BinarySerializerULong().encode(makeWriter(), BigInt('18446744073709551616'))]
    ];

    cases.forEach(([name, encode]) => {
        it(`should reject ${name}`, () => {
            expect(encode).toThrow(RangeError);
        });
    });

    it('should reject fractional integer values', () => {
        expect(() => new BinarySerializerInt().encode(makeWriter(), 1.5)).toThrow(RangeError);
    });

    it('should reject NaN', () => {
        expect(() => new BinarySerializerInt().encode(makeWriter(), NaN)).toThrow(RangeError);
    });

    it('should reject infinity', () => {
        expect(() => new BinarySerializerInt().encode(makeWriter(), Infinity)).toThrow(RangeError);
    });

    it('should reject a number passed to a bigint serializer at runtime', () => {
        expect(() => new BinarySerializerLong().encode(makeWriter(), 1 as unknown as bigint)).toThrow(RangeError);
    });
});
