/**
 * Thrown when an argument is null.
 *
 * @public
 * @exported
 * @extends {Error}
 */
export class ArgumentNullException extends Error {
    constructor(name: string) {
        super(`The '${ name }' value cannot be null.`);

        this.name = new.target.name;
    }
}
