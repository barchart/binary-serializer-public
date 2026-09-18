import * as is from '@barchart/common-js/lang/is';

export function assertIntegerInRange(value: number, minimum: number, maximum: number): void {
    if (!is.large(value) || value < minimum || value > maximum) {
        throw new RangeError(`The value must be an integer between ${ minimum } and ${ maximum }. The value was ${ value }.`);
    }
}

export function assertBigIntInRange(value: bigint, minimum: bigint, maximum: bigint): void {
    if (typeof value !== 'bigint' || value < minimum || value > maximum) {
        throw new RangeError(`The value must be a bigint between ${ minimum } and ${ maximum }. The value was ${ value }.`);
    }
}
