namespace Barchart.BinarySerializer.Examples.Scripts;

/// <summary>
///     Represents a script with a unique execution logic.
/// </summary>
public interface IScript
{
    #region Properties

    /// <summary>
    ///     Script enumeration value that identifies the type of script.
    /// </summary>
    Script Script { get; }
    
    #endregion

    #region Methods

    /// <summary>
    ///    Executes the (de)serialization example logic for the script.
    /// </summary>
    void Execute();
    
    #endregion
}