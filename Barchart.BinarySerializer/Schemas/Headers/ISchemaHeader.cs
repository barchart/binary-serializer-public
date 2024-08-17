namespace Barchart.BinarySerializer.Schemas.Headers;

/// <summary>
///     <para>
///         Describes data which is always included when an entity is
///         serialized.
///     </para>
///     <para>
///         This interface could be extended to include additional
///         properties -- like a message timestamp, a message sequence
///         number, the byte length of the message, etc. Obviously, doing
///         so would add to the total length of the message. Consumers
///         of this library could extend this interface and write a
///         custom <see cref="ISchemaHeaderSerializer{THeader}" /> as
///         desired.
///     </para>
/// </summary>
public interface ISchemaHeader
{
    /// <summary>
    ///     An identifier for the entity that was serialized.
    /// </summary>
    byte EntityId { get; }

    /// <summary>
    ///     True if the message is a "snapshot" and contains all
    ///     properties of the entity. False if the message is
    ///     a "delta" and omits properties that have not changed.
    /// </summary>
    bool Snapshot { get; }
}