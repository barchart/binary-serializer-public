/**
 * Thrown when an entity ID is invalid (i.e. [ 0 ]).
 *
 * @public
 * @exported
 * @extends {Error}
 */
export class InvalidEntityIdException extends Error {
    constructor() {
        super("Entity ID cannot be [ 0 ].");

        Object.setPrototypeOf(this, InvalidEntityIdException.prototype);
    }
}
