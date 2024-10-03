/**
 * Provides utilities methods for handling byte arrays and bit operations in the context of binary serialization tests.
 *
 * @public
 * @exported
 */
export class Helpers {
    /**
     * Combines the first four byte arrays from a list into a single byte array.
     * The method concatenates these byte arrays in the order they appear in the list.
     * It throws an exception if the list does not contain at least four byte arrays.
     *
     * @public
     * @static
     * @param byteArrays - A list containing byte arrays to be combined.
     * @returns A byte array that is the result of concatenating the first four byte arrays from the list.
     * @throws {Error} Thrown if the list contains fewer than four byte arrays.
     */
    static combineFourByteArrays(byteArrays: Uint8Array[]): Uint8Array {
        if (!byteArrays || byteArrays.length < 4) {
            throw new Error("The list must contain at least four byte arrays.");
        }

        let totalLength = 0;
        for (let i = 0; i < 4; i++) {
            totalLength += byteArrays[i].length;
        }

        const combinedArray = new Uint8Array(totalLength);
        let offset = 0;

        for (let i = 0; i < 4; i++) {
            combinedArray.set(byteArrays[i], offset);
            offset += byteArrays[i].length;
        }

        return combinedArray;
    }
}