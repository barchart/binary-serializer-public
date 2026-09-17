import { describe, it, expect } from 'vitest';
import Day from '@barchart/common-js/lang/Day';
import Big from 'big.js';

import { EntityKey } from '../../../src/entities/keys/entity-key';

class TestEntity {}

describe('EntityKey', () => {
    describe('Equals with object', () => {
        it('should return false for another key implementation', () => {
            const key = new EntityKey<TestEntity>(['Luka', 1]);
            const other = { equals: () => true };

            expect(key.equals(other)).toBe(false);
        });

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

        it('should return false for different arrays with different string value', () => {
            const keyOne = new EntityKey<TestEntity>(['Luka', 1]);
            const keyTwo = new EntityKey<TestEntity>(['Bryan', 1]);
            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should return false for different arrays with different number value', () => {
            const keyOne = new EntityKey<TestEntity>(['Luka', 1]);
            const keyTwo = new EntityKey<TestEntity>(['Luka', 2]);
            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should support equal bigint values', () => {
            const keyOne = new EntityKey<TestEntity>([BigInt(1)]);
            const keyTwo = new EntityKey<TestEntity>([BigInt(1)]);

            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should distinguish different bigint values', () => {
            const keyOne = new EntityKey<TestEntity>([BigInt(1)]);
            const keyTwo = new EntityKey<TestEntity>([BigInt(2)]);

            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should treat NaN values as equal', () => {
            const keyOne = new EntityKey<TestEntity>([NaN]);
            const keyTwo = new EntityKey<TestEntity>([NaN]);

            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should distinguish NaN from Infinity', () => {
            const keyOne = new EntityKey<TestEntity>([NaN]);
            const keyTwo = new EntityKey<TestEntity>([Infinity]);

            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should distinguish NaN from null', () => {
            const keyOne = new EntityKey<TestEntity>([NaN]);
            const keyTwo = new EntityKey<TestEntity>([null]);

            expect(keyOne.equals(keyTwo)).toBe(false);
        });

        it('should compare dates by value', () => {
            const keyOne = new EntityKey<TestEntity>([new Date(1234)]);
            const keyTwo = new EntityKey<TestEntity>([new Date(1234)]);

            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should compare date-only values by value', () => {
            const keyOne = new EntityKey<TestEntity>([new Day(2026, 9, 18)]);
            const keyTwo = new EntityKey<TestEntity>([new Day(2026, 9, 18)]);

            expect(keyOne.equals(keyTwo)).toBe(true);
        });

        it('should compare decimal values by value', () => {
            const keyOne = new EntityKey<TestEntity>([new Big('1.0')]);
            const keyTwo = new EntityKey<TestEntity>([new Big('1.00')]);

            expect(keyOne.equals(keyTwo)).toBe(true);
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
