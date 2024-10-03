import { DataBufferReader } from "../data-buffer-reader";
import { DataReader } from "../data-reader.interface";
import { DataReaderFactory } from "./data-reader-factory.interface";

/**
 * An implementation of DataReaderFactory that uses
 * byte arrays as the underlying binary data storage for
 * each DataReader.
 *
 * @public
 * @exported
 * @implements {DataReaderFactory}
 */
export class DataBufferReaderFactory implements DataReaderFactory {
    make(bytes: Uint8Array): DataReader {
        return new DataBufferReader(bytes);
    }
}