﻿namespace Barchart.BinarySerializer.Attributes;

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

    public SerializeAttribute(bool key = false)
    {
        Key = key;
    }

    #endregion
}