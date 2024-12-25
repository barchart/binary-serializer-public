import { BinaryTypeSerializer } from './binary-type-serializer.interface';
import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';
import { BinarySerializerUShort } from './binary-serializer-ushort';
import {InvalidStringLengthException} from "./exceptions/invalid-string-length-exception";

/**
 * Reads (and writes) string values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @implements {BinaryTypeSerializer<string | null>}
 */
export class BinarySerializerString implements BinaryTypeSerializer<string | null> {
    private static readonly MAX_STRING_LENGTH_IN_BYTES = 2**16 - 1;
    private readonly binarySerializerUShort: BinarySerializerUShort;
    private readonly encoding: typeof TextEncoder;

    constructor(encoding: typeof TextEncoder = TextEncoder) {
        this.binarySerializerUShort = new BinarySerializerUShort();
        this.encoding = encoding;
    }

    /**
     * @throws {InvalidStringLengthException} The string length exceeds the maximum allowed length.
     */
    encode(writer: DataWriter, value: string | null): void {
        if (value === null) {
            this.writeNullFlag(writer, true);
            return;
        }

        this.writeNullFlag(writer, false);

        const encoder = new this.encoding();
        const bytes = encoder.encode(value);

        if (bytes.length > BinarySerializerString.MAX_STRING_LENGTH_IN_BYTES) {
            throw new InvalidStringLengthException(bytes.length, BinarySerializerString.MAX_STRING_LENGTH_IN_BYTES);
        }

        this.binarySerializerUShort.encode(writer, bytes.length);
        writer.writeBytes(bytes);
    }
  
    decode(reader: DataReader): string | null {
        if (this.readNullFlag(reader)) {
            return null;
        }

        const length = this.binarySerializerUShort.decode(reader);
        const bytes = reader.readBytes(length);

        return new TextDecoder().decode(bytes);
    }

    getEquals(a: string | null, b: string | null): boolean {
        if (a === null && b === null) {
            return true;
        }

        if (a === null || b === null) {
            return false;
        }

        return a === b;
    }

    private readNullFlag(reader: DataReader): boolean {
        return reader.readBit();
    }

    private writeNullFlag(writer: DataWriter, flag: boolean): void {
        writer.writeBit(flag);
    }
}