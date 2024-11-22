import { Serializer, DataType, SchemaField } from "../../src";

class TestEntity {
    keyProperty: string = "";
    valueProperty: string = "";
}

describe('SerializerTests', () => {
    let serializer: Serializer<TestEntity>;

    const entityId = 1;
    const fields: SchemaField[] = [
        { name: 'keyProperty', type: DataType.string, isKey: true },
        { name: 'valueProperty', type: DataType.string, isKey: false }
    ];

    beforeEach(() => {
        serializer = new Serializer<TestEntity>(entityId, fields);
    });

    describe('Serialize', () => {
        it('Serialize_SingleEntity_ReturnsSerializedData', () => {
            const entity = new TestEntity();
            entity.keyProperty = "Key";
            entity.valueProperty = "Value";

            const serialized = serializer.serialize(entity);

            expect(serialized).not.toBeNull();
            expect(serialized.length).not.toBe(0);
        });

        it('Serialize_Changes_ReturnsSerializedData', () => {
            const currentEntity = new TestEntity();
            currentEntity.keyProperty = "Key";
            currentEntity.valueProperty = "Value1";

            const previousEntity = new TestEntity();
            previousEntity.keyProperty = "Key";
            previousEntity.valueProperty = "Value0";

            const serialized = serializer.serializeChanges(currentEntity, previousEntity);

            expect(serialized).not.toBeNull();
            expect(serialized.length).not.toBe(0);
        });
    });

    describe('Deserialize', () => {
        it('Deserialize_SingleEntity_ReturnsDeserializedEntity', () => {
            const entity = new TestEntity();
            entity.keyProperty = "Key";
            entity.valueProperty = "Value";

            const serialized = serializer.serialize(entity);
            const deserializedEntity = serializer.deserialize(serialized);

            expect(deserializedEntity).not.toBeNull();
            expect(deserializedEntity.keyProperty).toBe(entity.keyProperty);
            expect(deserializedEntity.valueProperty).toBe(entity.valueProperty);
        });

        it('Deserialize_Changes_ShouldPopulateTargetEntity', () => {
            const entity = new TestEntity();
            entity.keyProperty = "Key";
            entity.valueProperty = "Value";

            const serialized = serializer.serialize(entity);
            const targetEntity = new TestEntity();
            targetEntity.keyProperty = "Key";
            targetEntity.valueProperty = "";

            const deserializedEntity = serializer.deserializeChanges(serialized, targetEntity);

            expect(deserializedEntity).not.toBeNull();
            expect(deserializedEntity).toEqual(targetEntity);
            expect(deserializedEntity.keyProperty).toBe(entity.keyProperty);
            expect(deserializedEntity.valueProperty).toBe(entity.valueProperty);
        });
    });

    describe('ReadHeader', () => {
        it('ReadHeader_WhenCalled_ReturnsCorrectHeader', () => {
            const entity = new TestEntity();
            entity.keyProperty = "Key";
            entity.valueProperty = "Value";

            const serialized = serializer.serialize(entity);
            const expectedHeader = { entityId, snapshot: true };

            const header = serializer.readHeader(serialized);

            expect(header.entityId).toEqual(expectedHeader.entityId);
            expect(header.snapshot).toEqual(expectedHeader.snapshot);
        });
    });

    describe('ReadKey', () => {
        it('ReadKey_WhenCalled_ReturnsCorrectKey', () => {
            const entity = new TestEntity();
            entity.keyProperty = "Key";
            entity.valueProperty = "Value";

            const serialized = serializer.serialize(entity);

            const keyName = "keyProperty";
            const expectedKey = "Key";

            const result = serializer.readKey<string>(serialized, keyName);

            expect(result).toBe(expectedKey);
        });
    });

    describe('GetEquals', () => {
        it('GetEquals_EqualEntities_ReturnsTrue', () => {
            const entityA = new TestEntity();
            entityA.keyProperty = "Key";
            entityA.valueProperty = "Value";

            const entityB = new TestEntity();
            entityB.keyProperty = "Key";
            entityB.valueProperty = "Value";

            const result = serializer.getEquals(entityA, entityB);

            expect(result).toBe(true);
        });

        it('GetEquals_DifferentEntities_ReturnsFalse', () => {
            const entityA = new TestEntity();
            entityA.keyProperty = "Key";
            entityA.valueProperty = "Value";

            const entityB = new TestEntity();
            entityB.keyProperty = "Key";
            entityB.valueProperty = "DifferentValue";

            const result = serializer.getEquals(entityA, entityB);

            expect(result).toBe(false);
        });
    });
});