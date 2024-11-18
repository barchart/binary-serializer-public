import { DataWriter } from "./data-writer.interface";
import { InsufficientCapacityException } from "./exceptions/insufficient-capacity-exception";

/**
 * A data buffer writer which uses a fixed-length byte array to read data.
 *
 * @public
 * @exported
 * @implements {DataWriter}
 * @param {Uint8Array} byteArray - The byte array to write to.
 */
export class DataBufferWriter implements DataWriter {
    private static readonly TRUE: number = 1;

    private readonly byteArray: Uint8Array;

    private positionByte: number;
    private positionBit: number;

    isAtRootNestingLevel: boolean;

    constructor(byteArray: Uint8Array) {
        this.isAtRootNestingLevel = true;
        
        this.byteArray = byteArray;

        this.positionByte = 0;
        this.positionBit = 0;
    }


    writeBit(value: boolean): void {
        if (this.capacityWouldBeExceeded(0)) {
            throw new InsufficientCapacityException(true);
        }

        if (this.positionBit === 0) {
            this.byteArray[this.positionByte] = 0;
        }

        if (value) {
            const byteCurrent = this.byteArray[this.positionByte];
            const byteMask = DataBufferWriter.TRUE << (7 - this.positionBit);

            this.byteArray[this.positionByte] = byteCurrent | byteMask;
        }

        this.advanceBit();
    }

    writeByte(value: number): void {
        if (this.capacityWouldBeExceeded(this.positionBit === 0 ? 0 : 1)) {
            throw new InsufficientCapacityException(true);
        }

        if (this.positionBit === 0) {
            this.byteArray[this.positionByte++] = value & 0xFF;

            return;
        }

        const byteExisting: number = this.byteArray[this.positionByte];

        const byteFirstMask: number = value >> this.positionBit;
        this.byteArray[this.positionByte] = (byteExisting | byteFirstMask) & 0xFF;

        const bitsAppendedToFirstByte: number = 8 - this.positionBit;

        this.byteArray[++this.positionByte] = (value << bitsAppendedToFirstByte) & 0xFF;
    }

    writeBytes(value: Uint8Array): void {
        if (value.length === 0) {
            return;
        }

        if (value.length === 1) {
            this.writeByte(value[0]);

            return;
        }

        if (this.capacityWouldBeExceeded(this.positionBit === 0 ? value.length - 1 : value.length)) {
            throw new InsufficientCapacityException(true);
        }

        if (this.positionBit === 0) {

            for (let i = 0; i < value.length; i++) {
                this.byteArray[this.positionByte++] = (value[i]) & 0xFF;
            }
            
            return;
        }

        const byteFirst: number = this.byteArray[this.positionByte];
        const byteFirstMask: number = value[0] >> this.positionBit;

        this.byteArray[this.positionByte++] = (byteFirst | byteFirstMask) & 0xFF;

        const bitsAppendedToFirstByte: number = 8 - this.positionBit;

        for (let i = 1; i < value.length; i++) {
            const byteStart: number = value[i - 1] << bitsAppendedToFirstByte;
            const byteEnd: number = value[i] >> this.positionBit;

            this.byteArray[this.positionByte++] = (byteStart | byteEnd) & 0xFF;
        }

        this.byteArray[this.positionByte] = (value[value.length - 1] << bitsAppendedToFirstByte) & 0xFF;
    }

    toBytes(): Uint8Array {
        const byteCount: number = this.positionByte + (this.positionBit === 0 ? 0 : 1);
        
        return this.byteArray.slice(0, byteCount);
    }

    bytesWritten(): number {
        return this.positionBit === 0 ? this.positionByte : this.positionByte + 1;
    }

    bookmark(): WriterBookmark {
        return new WriterBookmark(this, this.positionByte, this.positionBit);
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
 * A bookmark for a `DataBufferWriter` which can be used to reset the writing position of the writer.
 *
 * @public
 * @exported
 * @param {DataBufferWriter} writer The writer to bookmark.
 * @param {number} positionByte The byte position to bookmark.
 * @param {number} positionBit The bit position to bookmark.
 */
export class WriterBookmark {
    private readonly writer: DataBufferWriter;

    private readonly positionByte: number;
    private readonly positionBit: number;

    private disposed: boolean;

    constructor(writer: DataBufferWriter, positionByte: number, positionBit: number) {
        this.writer = writer;

        this.positionByte = positionByte;
        this.positionBit = positionBit;

        this.disposed = false;
    }

    /**
     * Resets the writer to the position of the bookmark.
     *
     * @public
     */
    dispose(): void {
        if (this.disposed) {
            return;
        }

        this.disposed = true;

        this.writer['positionByte'] = this.positionByte;
        this.writer['positionBit'] = this.positionBit;
    }
}