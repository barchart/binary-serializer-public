import { describe, it, expect } from 'vitest';
import { EntityKey } from '../../../src/entities/keys/entity-key';

class TestEntity {}

describe('EntityKey', () => {
    describe('Equals with object', () => {
        it('should return true for the same object', () => {
            const same = {};
            const keyOne = new EntityKey<TestEntity>(same);
            const keyTwo = new EntityKey<TestEntity>(same);
            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should return false for different objects', () => {
            const objectOne = {};
            const objectTwo = {};

            const keyOne = new EntityKey<TestEntity>(objectOne);
            const keyTwo = new EntityKey<TestEntity>(objectTwo);
            expect(keyOne.equals(keyTwo)).toBe(false);
        });
    });

    describe('Equals with array', () => {
        it('should return true for the same array', () => {
            const same = ['Luka', 1];
            const keyOne = new EntityKey<TestEntity>(same);
            const keyTwo = new EntityKey<TestEntity>(same);
            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should return true for different arrays with the same values', () => {
            const keyOne = new EntityKey<TestEntity>(['Luka', 1]);
            const keyTwo = new EntityKey<TestEntity>(['Luka', 1]);
            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should return false for different arrays with different key value', () => {
            const keyOne = new EntityKey<TestEntity>(['Luka', 1]);
            const keyTwo = new EntityKey<TestEntity>(['Bryan', 1]);
            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should return false for different arrays with different key value', () => {
            const keyOne = new EntityKey<TestEntity>(['Luka', 1]);
            const keyTwo = new EntityKey<TestEntity>(['Luka', 2]);
            expect(keyOne.equals(keyTwo)).toBe(false);
        });
    });

    describe('ToString', () => {
        it('should end with key toString', () => {
            const mock = { toString: () => 'The End' };
            const key = new EntityKey<TestEntity>(mock);
            expect(key.toString()).toContain('(key=The End)');
        });
    });
});