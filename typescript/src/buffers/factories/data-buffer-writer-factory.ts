import { DataWriter } from "../data-writer.interface";
import { DataWriterFactory } from "./data-writer-factory.interface";
import { DataBufferWriter } from "../data-buffer-writer";
import { InvalidByteArrayLengthException } from "../exceptions/invalid-byte-array-length-exception";

/**
 * An implementation of `DataWriterFactory` that uses byte arrays as the underlying binary data storage for each DataWriter.
 *
 * @public
 * @exported
 * @implements {DataWriterFactory}
 * @param {number} byteArrayLength - The length of the byte array to use for each DataWriter.
 * @throws {InvalidByteArrayLengthException} If the byte array length is less than 1.
 */
export class DataBufferWriterFactory implements DataWriterFactory {
    private static readonly DEFAULT_BYTE_ARRAY_LENGTH: number = 512 * 1024;
    private static byteArray?: Uint8Array;

    private readonly byteArrayLength: number;

    constructor(byteArrayLength: number = DataBufferWriterFactory.DEFAULT_BYTE_ARRAY_LENGTH) {
        if (byteArrayLength < 1) {
            throw new InvalidByteArrayLengthException(byteArrayLength);
        }
        
        this.byteArrayLength = byteArrayLength;
    }

    make(): DataWriter {
        if (!DataBufferWriterFactory.byteArray) {
            DataBufferWriterFactory.byteArray = new Uint8Array(this.byteArrayLength);
        }

        return new DataBufferWriter(DataBufferWriterFactory.byteArray);
    }
}