import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinarySerializerInt } from "./binary-serializer-int";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import Decimal from 'decimal.js';

/**
 * Reads (and writes) decimal values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<Decimal>}
 */
export class BinarySerializerDecimal implements BinaryTypeSerializer<Decimal> {
    private binarySerializerInt: BinarySerializerInt;

    constructor() {
        this.binarySerializerInt = new BinarySerializerInt();
    }
    
    get sizeInBytes(): number {
        return 16;
    }

    encode(writer: DataWriter, value: Decimal): void {
        const components = this.getDecimalComponents(value);

        this.binarySerializerInt.encode(writer, components[0]);
        this.binarySerializerInt.encode(writer, components[1]);
        this.binarySerializerInt.encode(writer, components[2]);
        this.binarySerializerInt.encode(writer, components[3]);
    }

    decode(reader: DataReader): Decimal {
        const components = [
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
            this.binarySerializerInt.decode(reader),
        ];

        return this.constructDecimalFromComponents(components);
    }
    
    getEquals(a: Decimal, b: Decimal): boolean {
        return a.equals(b);
    }

    getDecimalComponents(value: Decimal): number[] {
        const parts = value.toFixed().split('.');
        const integerPart = new Decimal(parts[0]);
        const fractionalPart = parts[1] ? new Decimal(`0.${parts[1]}`) : new Decimal(0);
        
        const sign = value.isNegative() ? -1 : 1;
        const precision = fractionalPart.decimalPlaces();
        
        const scaledInteger = integerPart.abs().times(Decimal.pow(10, precision));
        const scaledFractional = fractionalPart.times(Decimal.pow(10, precision));
        
        const combinedScaled = scaledInteger.plus(scaledFractional);
        
        const highBits = combinedScaled.divToInt(Decimal.pow(2, 32)).toNumber();
        const lowBits = combinedScaled.mod(Decimal.pow(2, 32)).toNumber();

        return [sign, highBits, lowBits, precision];
    }

    private constructDecimalFromComponents(components: number[]): Decimal {
        const [sign, highBits, lowBits, precision] = components;
        
        const combinedScaled = new Decimal(highBits).times(Decimal.pow(2, 32)).plus(lowBits);
        let result = combinedScaled.div(Decimal.pow(10, precision));

        if (sign === -1) {
            result = result.negated();
        }

        return result;
    }
}