using Barchart.BinarySerializer.Attributes;

namespace Barchart.BinarySerializer.Entities.Exceptions;

/// <summary>
///     An exception thrown when an entity type does not have any properties or fields marked as keys with the <see cref="SerializeAttribute"/>.
/// </summary>
public class MissingKeyMembersException : InvalidOperationException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MissingKeyMembersException"/> class.
    /// </summary>
    /// <param name="entityType">
    ///     The type of the entity.
    /// </param>
    public MissingKeyMembersException(Type entityType) : base($"The entity type '{entityType.Name}' does not have any properties or fields marked as keys with the {nameof(SerializeAttribute)}'.")
    {
        
    }
    
}