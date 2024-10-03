import { DataBufferReader, DataBufferWriter, DataReader, DataWriter, SchemaItemList } from "../../../src";

describe('SchemaItemListTests', () => {
    let writer: DataWriter;
    let reader: DataReader;
    let mockSchema: any;
    let schemaItemList: SchemaItemList<TestEntity, TestProperty>;

    beforeEach(() => {
        const buffer = new Uint8Array(100000);
        writer = new DataBufferWriter(buffer);
        reader = new DataBufferReader(buffer);

        mockSchema = {
            serialize: vi.fn(),
            serializeWithPrevious: vi.fn(),
            deserialize: vi.fn(),
            deserializeInto: vi.fn(),
            readHeader: vi.fn(),
            readKey: vi.fn(),
            getEquals: vi.fn()
        };

        schemaItemList = new SchemaItemList<TestEntity, TestProperty>(
            'listProperty',
            mockSchema
        );
    });

    describe('Encode', () => {
        it('calls schema serialize for each item when collection property is not null', () => {
            const testEntity: TestEntity = {
                listProperty: [
                    { propertyName: 'Test1', propertyValue: 123 },
                    { propertyName: 'Test2', propertyValue: 456 },
                ],
            };

            schemaItemList.encode(writer, testEntity);

            expect(mockSchema.serialize).toHaveBeenCalledTimes(2);
            expect(mockSchema.serialize).toHaveBeenCalledWith(writer, {
                propertyName: 'Test1',
                propertyValue: 123,
            }, true);
            expect(mockSchema.serialize).toHaveBeenCalledWith(writer, {
                propertyName: 'Test2',
                propertyValue: 456,
            }, true);
        });

        it('writes null flag when collection property is null', () => {
            const testEntity: TestEntity = {
                listProperty: null,
            };

            schemaItemList.encode(writer, testEntity);

            expect(reader.readBit()).toBe(false);
            expect(reader.readBit()).toBe(true);
        });
    });

    describe('Decode', () => {
        it('calls schema deserialize for each item when collection property is not null', () => {
            const expectedItemsCount = 2;
            let count = 1;

            writer.writeBit(false);
            writer.writeBit(false);
            writer.writeBytes(new Uint8Array([expectedItemsCount]));

            const testEntity: TestEntity = {
                listProperty: [],
            };

            vi.spyOn(mockSchema, 'deserialize').mockImplementation(() => {
                const item = { propertyName: 'Test', propertyValue: count++ };
                testEntity.listProperty!.push(item);
                return item;
            });

            schemaItemList.decode(reader, testEntity);

            expect(mockSchema.deserialize).toHaveBeenCalledTimes(expectedItemsCount);
            expect(testEntity.listProperty?.length).toBe(expectedItemsCount);

            for (let i = 0; i < expectedItemsCount; i++) {
                expect(testEntity.listProperty![i]?.propertyValue).toBe(i + 1);
            }
        });

        it('sets property to null when collection property is null', () => {
            writer.writeBit(true);

            const testEntity: TestEntity = {
                listProperty: [],
            };

            schemaItemList.decode(reader, testEntity);

            expect(testEntity.listProperty?.length).toBe(0);
        });
    });

    describe('GetEquals', () => {
        it('returns true when both lists are null', () => {
            const entityA: TestEntity = { listProperty: null };
            const entityB: TestEntity = { listProperty: null };

            const result = schemaItemList.getEquals(entityA, entityB);

            expect(result).toBe(true);
        });

        it('returns true when both lists are empty', () => {
            const entityA: TestEntity = { listProperty: [] };
            const entityB: TestEntity = { listProperty: [] };

            const result = schemaItemList.getEquals(entityA, entityB);

            expect(result).toBe(true);
        });

        it('returns true when items are the same', () => {
            const entityA: TestEntity = {
                listProperty: [
                    { propertyName: 'Test1', propertyValue: 123 },
                    { propertyName: 'Test2', propertyValue: 456 },
                ],
            };

            const entityB: TestEntity = {
                listProperty: [
                    { propertyName: 'Test1', propertyValue: 123 },
                    { propertyName: 'Test2', propertyValue: 456 },
                ],
            };

            vi.spyOn(mockSchema, 'getEquals').mockImplementation((p1: any, p2: any) => p1.PropertyName === p2.PropertyName && p1.PropertyValue === p2.PropertyValue);

            const result = schemaItemList.getEquals(entityA, entityB);

            expect(result).toBe(true);
        });

        it('returns false when items are different', () => {
            const entityA: TestEntity = {
                listProperty: [
                    { propertyName: 'Test1', propertyValue: 123 },
                    { propertyName: 'Test2', propertyValue: 456 },
                ],
            };

            const entityB: TestEntity = {
                listProperty: [
                    { propertyName: 'Test3', propertyValue: 789 },
                    { propertyName: 'Test4', propertyValue: 101 },
                ],
            };

            const result = schemaItemList.getEquals(entityA, entityB);

            expect(result).toBe(false);
        });

        it('returns false when one list is null', () => {
            const entityA: TestEntity = { listProperty: null };
            const entityB: TestEntity = {
                listProperty: [{ propertyName: 'Test1', propertyValue: 123 }],
            };

            const result = schemaItemList.getEquals(entityA, entityB);

            expect(result).toBe(false);
        });
    });
});

class TestEntity {
    listProperty: TestProperty[] | null = [];
}

class TestProperty {
    propertyName: string = "";
    propertyValue: number = 0;
}