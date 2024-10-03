import { DataReader } from '../buffers/data-reader.interface';
import { DataWriter } from '../buffers/data-writer.interface';
import { BinaryTypeSerializer } from './binary-type-serializer.interface';

/**
 * Reads (and writes) characters to (and from) a binary data source.
 */
export class BinarySerializerChar implements BinaryTypeSerializer<string> {
    get sizeInBytes(): number {
        return 2;
    }

    encode(writer: DataWriter, value: string): void {
        if (value.length !== 1) {
            throw new Error('Value must be a single character');
        }

        const charCode = value.charCodeAt(0);
        const bytes = new Uint8Array(this.sizeInBytes);
        new DataView(bytes.buffer).setUint16(0, charCode, true);
        
        writer.writeBytes(bytes);
    }

    decode(reader: DataReader): string {
        const bytes = reader.readBytes(this.sizeInBytes);
        const charCode = new DataView(bytes.buffer).getUint16(0, true);
        
        return String.fromCharCode(charCode);
    }

    getEquals(a: string, b: string): boolean {
        if (a.length !== 1 || b.length !== 1) {
            throw new Error('Values must be single characters.');
        }
        return a === b;
    }
}