declare module '@barchart/common-js/lang/Day' {
    export default class Day {
        constructor(year: number, month: number, day: number);

        addDays(days: number, inverse?: boolean): Day;
        subtractDays(days: number): Day;
        addMonths(months: number, inverse?: boolean): Day;
        subtractMonths(months: number): Day;
        addYears(years: number, inverse?: boolean): Day;
        subtractYears(years: number): Day;
        getStartOfMonth(): Day;
        getEndOfMonth(): Day;
        getIsBefore(other: Day): boolean;
        getIsAfter(other: Day): boolean;
        getIsContained(first?: Day, last?: Day): boolean;
        getIsEqual(other: Day): boolean;
        getName(): string;
        get year(): number;
        get month(): number;
        get day(): number;
        format(): string;
        toJSON(): string;

        static clone(value: Day): Day;
        static parse(value: string): Day;
        static fromDate(date: Date): Day;
        static fromDateUtc(date: Date): Day;
        static getToday(): Day;
        static validate(year: number, month: number, day: number): boolean;
        static getDaysInMonth(year: number, month: number): number;
        static compareDays(a: Day, b: Day): number;
        static countDaysBetween(a: Day, b: Day): number;
    }
}