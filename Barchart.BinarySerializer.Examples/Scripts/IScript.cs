namespace Barchart.BinarySerializer.Examples.Scripts;

public interface IScript
{
    #region Properties

    Script Script { get; }
    
    #endregion

    #region Methods

    void Execute();
    
    #endregion
}