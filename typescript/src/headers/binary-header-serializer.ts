import { DataWriter } from "../buffers/data-writer.interface";
import { Header } from "./header";
import { InvalidHeaderException } from "./exceptions/invalid-header-exception";
import { DataReader } from "../buffers/data-reader.interface";

/**
 * Represents the binary header serializer responsible for encoding and decoding
 * headers in the binary data. A header typically contains metadata such as an entity ID
 * and snapshot information that is used to interpret the serialized data.
 *
 * @public
 * @exported
 */
export class BinaryHeaderSerializer {
    private static readonly SNAPSHOT_FLAG: number = 128;
    private static readonly MAX_ENTITY_ID: number = 15;

    /**
     * The singleton instance of the BinaryHeaderSerializer.
     *
     * @public
     * @static
     */
    public static readonly Instance = new BinaryHeaderSerializer();

    private constructor() {}

    /**
     * Serializes a header into the provided data buffer writer.
     *
     * @public
     * @param {DataWriter} writer - The data buffer writer to which the header will be written.
     * @param {number} entityId - The entity ID to be included in the header.
     * @param {boolean} snapshot - A boolean value indicating whether the data represents a snapshot.
     * @throws {RangeError} Thrown when the entityId argument exceeds the maximum value of 15.
     */
    encode(writer: DataWriter, entityId: number, snapshot: boolean): void {
        if (entityId > BinaryHeaderSerializer.MAX_ENTITY_ID) {
            throw new RangeError(`The entityId argument cannot exceed ${BinaryHeaderSerializer.MAX_ENTITY_ID} because the header serializer uses exactly four bits for entityId value.`);
        }

        let combined = entityId;

        if (snapshot) {
            combined = combined ^ BinaryHeaderSerializer.SNAPSHOT_FLAG;
        }

        writer.writeByte(combined);
    }

    /**
     * Deserializes a header from the provided data buffer reader.
     *
     * @public
     * @param {DataReader} reader - The data buffer reader from which the header will be read.
     * @returns {Header} A Header instance representing the decoded header.
     * @throws {InvalidHeaderException} Thrown when the entityId value exceeds the maximum value of 15.
     */
    decode(reader: DataReader): Header {
        const combined = reader.readByte();

        const snapshot = (combined & BinaryHeaderSerializer.SNAPSHOT_FLAG) === BinaryHeaderSerializer.SNAPSHOT_FLAG;

        let entityId = combined;

        if (snapshot) {
            entityId = entityId ^ BinaryHeaderSerializer.SNAPSHOT_FLAG;
        }

        if (entityId > BinaryHeaderSerializer.MAX_ENTITY_ID) {
            throw new InvalidHeaderException(BinaryHeaderSerializer.MAX_ENTITY_ID);
        }

        return new Header(entityId, snapshot);
    }
}