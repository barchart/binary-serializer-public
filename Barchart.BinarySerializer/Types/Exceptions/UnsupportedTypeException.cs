#region Using Statements

using Barchart.BinarySerializer.Types.Factories;

#endregion

namespace Barchart.BinarySerializer.Types.Exceptions;

/// <summary>
///     Thrown when an <see cref="IBinaryTypeSerializerFactory" /> is unable to
///     create an <see cref="IBinaryTypeSerializer{T}" /> for the requested type.
/// </summary>
public class UnsupportedTypeException : InvalidOperationException
{
    #region Fields

    private readonly Type _unsupported;

    #endregion
    
    #region Constructor(s)
    
    public UnsupportedTypeException(Type unsupported) : base($"Unable to create a serializer for the ({unsupported.Name}) type.")
    {
        _unsupported = unsupported;
    }
    
    #endregion
    
    #region Properties

    /// <summary>
    ///     The type for which an <see cref="IBinaryTypeSerializer{T}" /> cannot be created.
    /// </summary>
    public Type Unsupported => _unsupported;

    #endregion
}