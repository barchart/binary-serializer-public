import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerInt } from "./binary-serializer-int";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import Big from 'big.js';

/**
 * Reads (and writes) decimal values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Big>}
 */
export class BinarySerializerDecimal implements BinaryTypeSerializer<Big> {
    private binarySerializerInt: BinarySerializerInt;

    constructor() {
        this.binarySerializerInt = new BinarySerializerInt();
    }
    
    get sizeInBytes(): number {
        return 16;
    }

    encode(writer: DataWriter, value: Big): void {
        const components = this.getDecimalComponents(value);

        this.binarySerializerInt.encode(writer, components[0]);
        this.binarySerializerInt.encode(writer, components[1]);
        this.binarySerializerInt.encode(writer, components[2]);
        this.binarySerializerInt.encode(writer, components[3]);
    }

    decode(reader: DataReader): Big {
        const components = [
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
        ];

        return this.constructDecimalFromComponents(components);
    }
    
    getEquals(a: Big, b: Big): boolean {
        return a.eq(b);
    }

    getDecimalComponents(value: Big): number[] {
        const parts = value.toFixed().split('.');
        const precision = parts[1]?.length ?? 0;

        if (precision > 28) {
            throw new RangeError('Decimal precision cannot exceed 28 decimal places.');
        }

        const wordSize = new Big(2).pow(32);
        const combinedScaled = value.abs().times(new Big(10).pow(precision));
        const lowBits = combinedScaled.mod(wordSize);
        const remainingBits = combinedScaled.minus(lowBits).div(wordSize);
        const middleBits = remainingBits.mod(wordSize);
        const highBits = remainingBits.minus(middleBits).div(wordSize);

        if (highBits.gte(wordSize)) {
            throw new RangeError('Decimal value exceeds the 96-bit range.');
        }

        const components = [lowBits, middleBits, highBits].map(component => {
            return component.gt(0x7fffffff) ? component.minus(wordSize).toNumber() : component.toNumber();
        });
        const flags = precision * 0x10000 - (value.lt(0) ? 0x80000000 : 0);

        return [...components, flags];
    }

    private constructDecimalFromComponents(components: number[]): Big {
        const [lowBits, middleBits, highBits, flags] = components;
        const wordSize = new Big(2).pow(32);
        const low = new Big(lowBits < 0 ? lowBits + 0x100000000 : lowBits);
        const middle = new Big(middleBits < 0 ? middleBits + 0x100000000 : middleBits);
        const high = new Big(highBits < 0 ? highBits + 0x100000000 : highBits);
        const combinedScaled = high.times(wordSize).plus(middle).times(wordSize).plus(low);
        const precision = (flags >>> 16) & 0xff;
        let result = new Big(`${combinedScaled.toFixed()}e-${precision}`);

        if (flags < 0) {
            result = result.neg();
        }

        return result;
    }
}
