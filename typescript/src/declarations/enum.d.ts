declare module '@barchart/common-js/lang/Enum' {
    export default class Enum {
        constructor(code: string, description: string, mapping?: number);

        get code(): string;
        get description(): string;
        get mapping(): number | null;

        equals(other: Enum): boolean;
        toJSON(): string;

        static fromCode<T extends Enum>(type: new (...args: any[]) => T, code: string): T | null;
        static fromMapping<T extends Enum>(type: new (...args: any[]) => T, mapping: number): T | null;
        static getItems<T extends Enum>(type: new (...args: any[]) => T): T[];

        toString(): string;
    }
}