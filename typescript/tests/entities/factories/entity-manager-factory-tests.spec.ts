import { EntityManagerFactory, EntityManager, Serializer, SchemaField, DataType, EntityKey } from '../../../src';

class TestEntityOne {
    keyPropertyOne: string;

    constructor(keyPropertyOne: string) {
        this.keyPropertyOne = keyPropertyOne;
    }
}

class TestEntityTwo {
    keyPropertyOne: string;
    keyPropertyTwo: number;

    constructor(keyPropertyOne: string, keyPropertyTwo: number) {
        this.keyPropertyOne = keyPropertyOne;
        this.keyPropertyTwo = keyPropertyTwo;
    }
}

describe('EntityManagerFactory', () => {
    let factory: EntityManagerFactory;
    let serializerTestEntityOne: Serializer<TestEntityOne>;
    let serializerTestEntityTwo: Serializer<TestEntityTwo>;
    let fieldsOne: SchemaField[];
    let fieldsTwo: SchemaField[];

    beforeEach(() => {
        factory = new EntityManagerFactory();

        fieldsOne = [
            { name: 'keyPropertyOne', isKey: true, type: DataType.string }
        ];

        fieldsTwo = [
            { name: 'keyPropertyOne', isKey: true, type: DataType.string },
            { name: 'keyPropertyTwo', isKey: true, type: DataType.int }
        ];

        serializerTestEntityOne = new Serializer<TestEntityOne>(1, fieldsOne);
        serializerTestEntityTwo = new Serializer<TestEntityTwo>(2, fieldsTwo);
    });

    it('should create an EntityManager for TestEntityOne with the correct key extractor', () => {
        const entityManager = factory.make(serializerTestEntityOne, fieldsOne);

        expect(entityManager).toBeInstanceOf(EntityManager);
    });

    it('should create an EntityManager for TestEntityTwo with the correct key extractor', () => {
        const entityManager = factory.make(serializerTestEntityTwo, fieldsTwo);

        expect(entityManager).toBeInstanceOf(EntityManager);
    });
});