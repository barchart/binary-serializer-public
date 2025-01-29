import { EntityManagerFactory, EntityManager, Serializer, SchemaField, DataType, EntityNotFoundException } from '../../src';
import {expect} from "vitest";

class TestEntity {
    key: number;
    value: number;

    constructor(key: number, value: number) {
        this.key = key;
        this.value = value;
    }
}

class TestEntityTwo {
    key: number;
    keyTwo: string;
    value: number;

    constructor(key: number, keyTwo: string, value: number) {
        this.key = key;
        this.keyTwo = keyTwo;
        this.value = value;
    }
}

describe('EntityManager', () => {
    let serializer: Serializer<TestEntity>;
    let serializerTwo: Serializer<TestEntityTwo>;

    let entityManager: EntityManager<TestEntity>;
    let entityManagerTwo: EntityManager<TestEntityTwo>;

    beforeEach(() => {
        const fieldsOne: SchemaField[] = [
            { name: 'key', type: DataType.byte, isKey: true},
            { name: 'value', type: DataType.byte }
        ];

        const fieldsTwo: SchemaField[] = [
            { name: 'key', type: DataType.byte, isKey: true},
            { name: 'keyTwo', type: DataType.string, isKey: true},
            { name: 'value', type: DataType.byte }
        ];

        serializer = new Serializer<TestEntity>(1, fieldsOne);
        serializerTwo = new Serializer<TestEntityTwo>(2, fieldsTwo);

        const entityManagerFactory = new EntityManagerFactory();

        entityManager = entityManagerFactory.make<TestEntity>(serializer, fieldsOne);
        entityManagerTwo = entityManagerFactory.make<TestEntityTwo>(serializerTwo, fieldsTwo);
    });

    it('should create a snapshot and return a byte array', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);
        const snapshot = entityManager.snapshot(entity);

        expect(snapshot.length).toBe(4);
    });

    it('should create a snapshot for a compound key entity and return a byte array', () => {
        const entity = new TestEntityTwo(0b11110000, 'KeyTwo', 0b00001111);
        const snapshot = entityManagerTwo.snapshot(entity);

        expect(snapshot.length).toBe(12);
    });

    it('should throw EntityNotFoundException if there is no snapshot for the entity', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);
        expect(() => entityManager.difference(entity)).toThrow(EntityNotFoundException);
    });

    it('should throw EntityNotFoundException if there is no snapshot for the compound key entity', () => {
        const entity = new TestEntityTwo(0b11110000, 'KeyTwo', 0b00001111);
        expect(() => entityManagerTwo.difference(entity)).toThrow(EntityNotFoundException);
    });

    it('should return an empty byte array if there are no changes', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);

        entityManager.snapshot(entity);
        const difference = entityManager.difference(entity);

        expect(difference.length).toBe(0);
    });

    it('should return an empty byte array if there are no changes for the compound key entity', () => {
        const entity = new TestEntityTwo(0b11110000, 'KeyTwo', 0b00001111);

        entityManagerTwo.snapshot(entity);
        const difference = entityManagerTwo.difference(entity);

        expect(difference.length).toBe(0);
    });

    it('should return a byte array if there are changes', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);

        entity.value = 0b11111000;
        const difference = entityManager.difference(entity);

        expect(difference.length).toBe(4);
    });

    it('should return a byte array if there are changes for the compound key entity', () => {
        const entity = new TestEntityTwo(0b11110000, 'KeyTwo', 0b00001111);

        entityManagerTwo.snapshot(entity);
        entity.value = 0b11111000;
        const difference = entityManagerTwo.difference(entity);

        expect(difference.length).toBe(12);
    });

    it('should return an empty byte array if there are no changes after a second difference call', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);

        entityManager.snapshot(entity);
        entity.value = 0b11111000;
        entityManager.difference(entity);
        const difference = entityManager.difference(entity);

        expect(difference.length).toBe(0);
    });

    it('should return an empty byte array if there are no changes after a second difference call with mutation', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);

        entityManager.snapshot(entity);
        entity.value = 0b11111000;
        entityManager.difference(entity);
        entity.value = 0b11111100;
        const difference = entityManager.difference(entity);

        expect(difference.length).toBe(4);
    });

    it('should return false if trying to remove a non-existent snapshot', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);
        const removed = entityManager.remove(entity);

        expect(removed).toBe(false);
    });

    it('should return false if trying to remove a non-existent snapshot for the compound key entity', () => {
        const entity = new TestEntityTwo(0b11110000, 'KeyTwo', 0b00001111);
        const removed = entityManagerTwo.remove(entity);

        expect(removed).toBe(false);
    });

    it('should remove the snapshot and throw EntityNotFoundException on difference call', () => {
        const entity = new TestEntity(0b11110000, 0b00001111);
        entityManager.snapshot(entity);
        entityManager.remove(entity);

        expect(() => entityManager.difference(entity)).toThrow(EntityNotFoundException);
    });
});