import { DataReader } from "./data-reader.interface";
import { InsufficientCapacityException } from "./exceptions/insufficient-capacity-exception";

/**
 * A data buffer reader which uses a fixed-length byte array to read data.
 *
 * @public
 * @exported
 * @implements {DataReader}
 * @param {Uint8Array} byteArray - The byte array to read from.
 */
export class DataBufferReader implements DataReader {
    private readonly byteArray: Uint8Array;

    private positionByte: number;
    private positionBit: number;

    constructor(byteArray: Uint8Array) {
        this.byteArray = byteArray;
        
        this.positionByte = 0;
        this.positionBit = 0;
    }

    readBit(): boolean {
        if (this.capacityWouldBeExceeded(0)) {
            throw new InsufficientCapacityException(false);
        }

        const bit = (this.byteArray[this.positionByte] >> (7 - this.positionBit)) & 1;

        this.advanceBit();

        return bit === 1;
    }

    readByte(): number {
        if (this.capacityWouldBeExceeded(this.positionBit === 0 ? 0 : 1)) {
            throw new InsufficientCapacityException(false);
        }

        return this.readByteUnchecked();
    }

    readBytes(size: number): Uint8Array {
        if (size === 0) {
            return new Uint8Array(0);
        }

        if (this.capacityWouldBeExceeded(this.positionBit === 0 ? size - 1 : size)) {
            throw new InsufficientCapacityException(false);
        }

        const bytes = new Uint8Array(size);

        for (let i = 0; i < size; i++) {
            bytes[i] = this.readByteUnchecked();
        }

        return bytes;
    }

    private readByteUnchecked(): number {
        if (this.positionBit === 0) {
            return this.byteArray[this.positionByte++] & 0xFF;
        }

        const byteFirst = this.byteArray[this.positionByte];
        const byteSecond = this.byteArray[++this.positionByte];

        const byteStart = byteFirst << this.positionBit;
        const byteEnd = byteSecond >> (8 - this.positionBit);

        return (byteStart | byteEnd) & 0xFF;
    }

    bookmark(): ReaderBookmark {
        return new ReaderBookmark(this, this.positionByte, this.positionBit);
    }

    private advanceBit(): void {
        if (this.positionBit === 7) {
            this.positionBit = 0;
            this.positionByte++;
        } else {
            this.positionBit++;
        }
    }

    private capacityWouldBeExceeded(additionalBytes: number): boolean {
        return this.positionByte + additionalBytes >= this.byteArray.length;
    }
}

/**
 * A bookmark for a `DataBufferReader` which can be used to reset the read position of the reader.
 *
 * @public
 * @exported
 * @param {DataBufferReader} reader - The reader to create a bookmark for.
 * @param {number} positionByte - The byte position of the reader.
 * @param {number} positionBit - The bit position of the reader.
 */
export class ReaderBookmark {
    private readonly reader: DataBufferReader;

    private readonly positionByte: number;
    private readonly positionBit: number;

    private disposed: boolean;

    constructor(reader: DataBufferReader, positionByte: number, positionBit: number) {
        this.reader = reader;

        this.positionByte = positionByte;
        this.positionBit = positionBit;

        this.disposed = false;
    }

    /**
     * Resets the reader to the position of the bookmark.
     *
     * @public
     */
    dispose(): void {
        if (this.disposed) {
            return;
        }

        this.disposed = true;

        this.reader['positionByte'] = this.positionByte;
        this.reader['positionBit'] = this.positionBit;
    }
}