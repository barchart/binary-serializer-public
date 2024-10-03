import { DataBufferReader, DataBufferReaderFactory, DataReader } from '../../../src';

describe('DataBufferReaderFactoryTests', () => {
    let factory: DataBufferReaderFactory;

    beforeEach(() => {
        factory = new DataBufferReaderFactory();
    });

    describe('Make', () => {
        it('should return an DataBufferReader instance with the default factory', () => {
            const byteArray = new Uint8Array(10);
            const reader: DataReader = factory.make(byteArray);

            expect(reader).toBeInstanceOf(DataBufferReader);
        });
    });
});