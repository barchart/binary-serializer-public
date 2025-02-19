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
    #region Constructor(s)
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="UnsupportedTypeException" /> class.
    /// </summary>
    /// <param name="unsupported">
    ///     The type that is not supported.
    /// </param>
    public UnsupportedTypeException(Type unsupported) : base($"Unable to create a serializer for the ({unsupported.Name}) type.")
    {
        
    }
    
    #endregion
}