import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerInt } from "./binary-serializer-int";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import Big from "big.js";

const SIZE_IN_BYTES = 16;
const MAX_SCALE = 28;
const WORD_SIZE = new Big(2).pow(32);
const MAX_SIGNED_WORD = 0x7FFFFFFF;
const SIGN_FLAG = -0x80000000;
const SCALE_BIT_OFFSET = 16;
const SCALE_MASK = 0xFF;

/**
 * Reads (and writes) decimal values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Big>}
 */
export class BinarySerializerDecimal implements BinaryTypeSerializer<Big> {
    private readonly binarySerializerInt = new BinarySerializerInt();
    
    get sizeInBytes(): number {
        return SIZE_IN_BYTES;
    }

    encode(writer: DataWriter, value: Big): void {
        for (const component of this.getDecimalComponents(value)) {
            this.binarySerializerInt.encode(writer, component);
        }
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
        const scale = parts[1]?.length ?? 0;

        if (scale > MAX_SCALE) {
            throw new RangeError('Decimal precision cannot exceed 28 decimal places.');
        }

        const combinedScaled = value.abs().times(new Big(10).pow(scale));
        const lowBits = combinedScaled.mod(WORD_SIZE);
        const remainingBits = combinedScaled.minus(lowBits).div(WORD_SIZE);
        const middleBits = remainingBits.mod(WORD_SIZE);
        const highBits = remainingBits.minus(middleBits).div(WORD_SIZE);

        if (highBits.gte(WORD_SIZE)) {
            throw new RangeError('Decimal value exceeds the 96-bit range.');
        }

        const components = [lowBits, middleBits, highBits].map(component => {
            return component.gt(MAX_SIGNED_WORD) ? component.minus(WORD_SIZE).toNumber() : component.toNumber();
        });
        
        const flags = (scale << SCALE_BIT_OFFSET) | (value.lt(0) ? SIGN_FLAG : 0);

        return [ ...components, flags ];
    }

    private constructDecimalFromComponents(components: number[]): Big {
        const [ lowBits, middleBits, highBits, flags ] = components;
      
        const low = new Big(lowBits >>> 0);
        const middle = new Big(middleBits >>> 0);
        const high = new Big(highBits >>> 0);
      
        const combinedScaled = high.times(WORD_SIZE).plus(middle).times(WORD_SIZE).plus(low);
        const scale = (flags >>> SCALE_BIT_OFFSET) & SCALE_MASK;
    
        const result = new Big(`${combinedScaled.toFixed()}e-${scale}`);

        return flags < 0 ? result.neg() : result;
    }
}
