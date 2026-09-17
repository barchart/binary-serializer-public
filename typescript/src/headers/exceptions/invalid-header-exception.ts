/**
 * Thrown when a serialized header cannot be parsed.
 *
 * @public
 * @exported
 * @extends {Error}
 * @param {number} maxEntityId - The maximum entityId value that can be serialized.
 */
export class InvalidHeaderException extends Error {
    constructor(maxEntityId: number) {
        super(`The entityId must be an integer between 0 and ${ maxEntityId } because the header serializer uses exactly four bits for entityId value.`);

        this.name = new.target.name;
    }
}
