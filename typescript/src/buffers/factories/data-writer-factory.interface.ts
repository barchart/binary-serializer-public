import { DataWriter } from "../data-writer.interface";

/**
 * A strategy for creating DataWriter instances.
 *
 * @public
 * @exported
 */
export interface DataWriterFactory {
     /**
     * Creates a new DataWriter instance.
     *
     * @returns A new DataWriter instance.
     */
    make(): DataWriter;
}