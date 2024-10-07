import { DataReader } from "../buffers/data-reader.interface";
import { DataWriter } from "../buffers/data-writer.interface";
import { BinaryTypeSerializer } from "./binary-type-serializer.interface";
import { Serialization } from "../utilities/utilities";

/**
 * Reads (and writes) nullable values to (and from) a binary data source.
 *
 * @public
 * @exported
 * @template T - The type to serialize to binary (and deserialize from binary).
 * @implements {BinaryTypeSerializer<T | null>}
 */

export class BinarySerializerNullable<T> implements BinaryTypeSerializer<T | null> {

    private readonly typeSerializer: BinaryTypeSerializer<T>;

    constructor(typeSerializer: BinaryTypeSerializer<T>) {
        this.typeSerializer = typeSerializer;
    }

    encode(writer: DataWriter, value: T | null): void {
        Serialization.writeNullFlag(writer, value === null);

        if (value !== null)
        {
            this.typeSerializer.encode(writer, value);
        }
    }

    decode(reader: DataReader): T | null {
        if (Serialization.readNullFlag(reader))
        {
            return null;
        }

        return this.typeSerializer.decode(reader);
    }

    getEquals(a: T, b: T): boolean {
        return this.typeSerializer.getEquals(a, b);
    }
}