namespace Barchart.BinarySerializer.Attributes;

/// <summary>
///     An attribute that marks a property (or field) for binary
///     serialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SerializeAttribute : Attribute
{
    #region Properties
    
    /// <summary>
    ///     Indicates if the member is the unique key (or one component
    ///     of a compound key).
    /// </summary>
    public bool Key { get; }

    #endregion

    #region Constructor(s)

    /// <summary>
    ///     Creates a new <see cref="SerializeAttribute"/> instance.
    /// </summary>
    /// <param name="key">
    ///     Indicates if the member is the unique key or part of a compound key.
    /// </param>
    public SerializeAttribute(bool key = false)
    {
        Key = key;
    }

    #endregion
}